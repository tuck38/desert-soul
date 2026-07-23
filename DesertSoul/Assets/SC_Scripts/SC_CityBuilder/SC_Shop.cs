using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SC_Shop : MonoBehaviour
{
    public static Action<bool> OnToggleShop;

    [SerializeField] GameObject shopUIParent;
    //[SerializeField] GameObject enableShopButtonParent;
    [SerializeField] Transform buildingParent;
    [SerializeField] SC_MouseTracker purchaseCursor;
    [SerializeField] SC_GridMaker grid;
    [SerializeField] List<SC_Building> buildings;

    [SerializeField] List<bool> unlocked;
    [SerializeField] List<Button> buttons;
    [SerializeField] KeyCode OpenShopKey;
    [Tooltip("Used when clicking to place a building. This value alters the allowed distance from the center of a cell for the click to be registered")]
    [SerializeField] float distanceFromCenterOfCellAllowance = 1.0f;

    [SerializeField] GameObject buildingBig;

    [SerializeField] GameObject shopUI;

    [SerializeField] GameObject buildButton;
    [SerializeField] GameObject selectButton;

    [SerializeField] Text resourceX;
    [SerializeField] Text resourceY;

    [SerializeField] Text buildName;

    [Header("Shop Camera Values")]
    [SerializeField] SC_Camera cameraScript;
    [SerializeField] Transform shopCameraTrackingTarget;
    [SerializeField] float shopCameraDistance;

    [SerializeField] GameObject firstshopButton;

    private SC_Building buildingToPlace;

    private SC_Building selectedBuilding;

    private List<SC_Building> tentativlyPlacedBuildings = new List<SC_Building>();
    private List<SC_GridCell> tentativlyAllocatedGridCells = new List<SC_GridCell>();
    private List<SC_Building> tentativlyPlacedBuildingPrefabs = new List<SC_Building>();
    private List<Vector3> tentativlyPlacedBuildingSpawnPoint = new List<Vector3>();

    static List<SC_Building> buildingsPlaced;
    static List<Vector3> buildingsPlacedLocation;

    private void Start()
    {
        if (buildingsPlaced == null) buildingsPlaced = new List<SC_Building>();
        if (buildingsPlacedLocation == null) buildingsPlacedLocation = new List<Vector3>();

        for (int i = 0; i < buildingsPlaced.Count; i++)
        {
            Instantiate(buildingsPlaced[i], buildingsPlacedLocation[i],Quaternion.identity, buildingParent);
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && buildingToPlace != null) CheckForValidGridCell();
    }

    /// <summary>
    /// Enables player to place input building onto map
    /// </summary>
    /// <param name="building"></param>
    public void SelectBuildingToPlace(SC_Building building)
    {
        //selectionmode
        //if player has enough resources
        SC_Player_Prop.OnResourcesAmountChanged?.Invoke(ResouceTypes.STONE, building.MaterialCost.x * -1);
        SC_Player_Prop.OnResourcesAmountChanged?.Invoke(ResouceTypes.TWINE, building.MaterialCost.y * -1);
        buildingToPlace = building;
        buildingBig.GetComponent<Image>().sprite = buildingToPlace.BuildingSprite;
        buildingBig.SetActive(true);
        selectedBuilding = buildingToPlace;
        buildName.text = buildingToPlace.BuildingName;
        resourceX.text = buildingToPlace.MaterialCost.x.ToString();
        resourceY.text = buildingToPlace.MaterialCost.y.ToString();
    }

    public void PlaceObject()
    {
        //get input from outside
        if( buildingToPlace != null) 
        {
            CheckForValidGridCell();
        }
    }

    public void SelectBuildingToSpawn()
    {
        //buildmode
        buildingToPlace = selectedBuilding;
        purchaseCursor.SetBuildingDimensions(selectedBuilding.GetSpriteDims().x, selectedBuilding.GetSpriteDims().y);
        purchaseCursor.gameObject.SetActive(true);
        shopUI.SetActive(false);
        purchaseCursor.GetComponent<SpriteRenderer>().sprite = buildingToPlace.BuildingSprite;
        Cursor.visible = false;
        buildButton.SetActive(true);
        EventSystem.current.SetSelectedGameObject(buildButton);
    }

    public void BackToBuildingSelection()
    {
        purchaseCursor.gameObject.SetActive(false);
        shopUI.SetActive(true);
        Cursor.visible = true;
        buildButton.SetActive(false);
        selectButton.SetActive(false);
    }

    /// <summary>
    /// Makes Menu UI visible, removing the activation button from view
    /// </summary>
    public void OpenShopUI()
    {
        CheckShopUnlocks();
        shopUIParent.SetActive(true);
        //enableShopButtonParent.SetActive(false);
        grid.gameObject.SetActive(true);
        OnToggleShop?.Invoke(false);
        cameraScript.SetShopView(shopCameraTrackingTarget, shopCameraDistance);

        EventSystem.current.firstSelectedGameObject = firstshopButton;

        EventSystem.current.SetSelectedGameObject(firstshopButton);
    }

    /// <summary>
    /// Makes Menu UI invisible, bringing the activation button back into view
    /// </summary>
    public void CloseShopUI()
    {
        shopUIParent.SetActive(false);
        //enableShopButtonParent.SetActive(true);
        grid.gameObject.SetActive(false);
        OnToggleShop?.Invoke(true);
        cameraScript.DefaultView();

        foreach (var cell in tentativlyAllocatedGridCells)
        {
            cell.isOccupied = false;
        }
        foreach (var building in tentativlyPlacedBuildings)
        {
            Destroy(building.gameObject);
        }

        tentativlyAllocatedGridCells.Clear();
        tentativlyPlacedBuildings.Clear();
        tentativlyPlacedBuildingPrefabs.Clear();
        tentativlyPlacedBuildingSpawnPoint.Clear();
        buildButton.SetActive(false);
        selectButton.SetActive(false);
    }

    /// <summary>
    /// Finalize building placement
    /// </summary>
    public void FinalizeBuild()
    {
        buildingsPlaced.AddRange(tentativlyPlacedBuildingPrefabs);
        buildingsPlacedLocation.AddRange(tentativlyPlacedBuildingSpawnPoint);
        
        Color buildingColor;
        foreach (var building in tentativlyPlacedBuildings)
        {
            buildingColor = building.GetComponent<SpriteRenderer>().color;
            building.GetComponent<SpriteRenderer>().color = new Color(buildingColor.r, buildingColor.g, buildingColor.b, 1f);
        }

        tentativlyAllocatedGridCells.Clear();
        tentativlyPlacedBuildings.Clear();
        tentativlyPlacedBuildingPrefabs.Clear();
        tentativlyPlacedBuildingSpawnPoint.Clear();
        CloseShopUI();
    }

    /// <summary>
    /// Check the locked status of each building and update it's corresponding button
    /// </summary>
    void CheckShopUnlocks()
    {
        for(int i = 0; i < buttons.Count; i++)
        {
            buttons[i].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = buildings[i].MaterialCost.x.ToString();
            buttons[i].transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = buildings[i].MaterialCost.y.ToString();

            if (!unlocked[i]) 
            {
                buttons[i].interactable = false;
                buttons[i].gameObject.SetActive(false);
            }
            else if (buildings[i].MaterialCost.x > SC_Player_Prop.stoneMaterialCount || buildings[i].MaterialCost.y > SC_Player_Prop.twineMaterialCount) 
            {
                buttons[i].interactable = false;
            }
            else 
            {
                buttons[i].interactable = true;
                buttons[i].gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Finds grid cell closest to mouse click and check if occupied by another structure or not
    /// </summary>
    void CheckForValidGridCell()
    {
        SC_GridCell closestGridCell = null;
        float closestDistance = float.MaxValue;

        foreach (SC_GridCell cell in grid.GetGrid())
        {
            float distance = Vector2.Distance(cell.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestGridCell = cell;
            }
        }

        if (closestDistance > distanceFromCenterOfCellAllowance)
        {
            buildingToPlace = null;
            purchaseCursor.gameObject.SetActive(false);
            Cursor.visible = true;
            return;
        }

        if (!closestGridCell.isOccupied)
        {
            if (buildingToPlace.IsMulticell)
            {
                List<SC_GridCell> usableCells = CheckAdjacentCells(closestGridCell, buildingToPlace.Dimensions);
                if (usableCells.Count == buildingToPlace.Dimensions.x * buildingToPlace.Dimensions.y)
                {
                    //Debug.Log($"{usableCells[usableCells.Count - 1].transform.position - closestGridCell.transform.position}, {closestGridCell.transform.position - usableCells[usableCells.Count - 1].transform.position}");
                    Vector3 spawnPosition = closestGridCell.transform.position + ((usableCells[usableCells.Count - 1].transform.position - closestGridCell.transform.position) / 2.0f);

                    SC_Building spawnedBuilding = Instantiate(buildingToPlace, spawnPosition, Quaternion.identity, buildingParent);
                    tentativlyPlacedBuildings.Add(spawnedBuilding);
                    Color buildingColor = spawnedBuilding.GetComponent<SpriteRenderer>().color;
                    spawnedBuilding.GetComponent<SpriteRenderer>().color = new Color(buildingColor.r, buildingColor.g, buildingColor.b, 0.5f);
                    tentativlyPlacedBuildingPrefabs.Add(buildingToPlace);
                    tentativlyPlacedBuildingSpawnPoint.Add(spawnPosition);
                    buildingToPlace = null;
                    foreach(SC_GridCell cell in usableCells)
                    {
                        cell.isOccupied = true;
                        tentativlyAllocatedGridCells.Add(cell);
                    }
                    purchaseCursor.gameObject.SetActive(false);
                    Cursor.visible = true;
                }
            }
            else
            {
                SC_Building spawnedBuilding = Instantiate(buildingToPlace, closestGridCell.transform.position, Quaternion.identity, buildingParent);
                tentativlyPlacedBuildings.Add(spawnedBuilding);
                Color buildingColor = spawnedBuilding.GetComponent<SpriteRenderer>().color;
                spawnedBuilding.GetComponent<SpriteRenderer>().color = new Color(buildingColor.r, buildingColor.g, buildingColor.b, 0.5f);
                tentativlyPlacedBuildingPrefabs.Add(buildingToPlace);
                tentativlyPlacedBuildingSpawnPoint.Add(closestGridCell.transform.position);
                buildingToPlace = null;
                closestGridCell.isOccupied = true;
                tentativlyAllocatedGridCells.Add(closestGridCell);
                purchaseCursor.gameObject.SetActive(false);
                Cursor.visible = true;
            }
        }
    }

    /// <summary>
    /// For multi-cell buildings, checks cells, within the passed in dimensions, that are connected to the passed in cell
    /// </summary>
    /// <param name="selectedCell"></param>
    /// <param name="buildingDimensions"></param>
    /// <returns></returns>
    List<SC_GridCell> CheckAdjacentCells(SC_GridCell selectedCell, Vector2Int buildingDimensions)
    {
        List<SC_GridCell> cells = new List<SC_GridCell>();
        int selectedCellIndex = grid.GetGrid().IndexOf(selectedCell);
        Vector2Int gridArrayMovement = new Vector2Int(grid.GridStartCorner == GridStartLocation.UPPERLEFT || grid.GridStartCorner == GridStartLocation.LOWERLEFT ? 1 : -1,
                                                      grid.GridStartCorner == GridStartLocation.LOWERLEFT || grid.GridStartCorner == GridStartLocation.LOWERRIGHT ? 1 : -1);
        int previousIndex;
        for(int yIndex = 0; yIndex < buildingDimensions.y; yIndex++)
        {
            previousIndex = -1;
            for (int xIndex = 0; xIndex < buildingDimensions.x; xIndex++)
            {
                int nextIndex = selectedCellIndex + (xIndex * gridArrayMovement.x) + (yIndex * grid.GridDimensions.x * gridArrayMovement.y);
                int modDiff = (previousIndex % grid.GridDimensions.x) - (nextIndex % grid.GridDimensions.x);
                if (nextIndex >= 0 && nextIndex < grid.GetGrid().Count)
                {
                    if (xIndex == 0 || (Math.Abs(modDiff) == 1 && xIndex != 0))
                    {
                        if (!grid.GetGrid()[nextIndex].isOccupied)
                        {
                            //Debug.Log(grid.GetGrid()[nextIndex].name);
                            cells.Add(grid.GetGrid()[nextIndex]);
                        }
                    }
                }
                else return cells;
                previousIndex = nextIndex;
            }
        }
        return cells;
    }

    public void SetUnlocks(List<bool> unlocks)
    {
        unlocked = unlocks;
    }

    public List<bool> GetUnlocks()
    {
        return unlocked;
    }
}
