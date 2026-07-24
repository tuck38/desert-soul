using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using JetBrains.Annotations;

public class SC_Shop : MonoBehaviour
{
    public static Action<bool> OnToggleShop;

    [SerializeField] GameObject shopUIParent;
    //[SerializeField] GameObject enableShopButtonParent;
    [SerializeField] Transform buildingParent;
    [SerializeField] SC_MouseTracker purchaseCursor;
    [SerializeField] SC_GridMaker grid;
    [SerializeField] List<SC_Building> buildings;

    //So many bool arrays, bad practice lolol

    [SerializeField] List<bool> placed;

    [SerializeField] List<bool> oneTime;

    [SerializeField] List<GameObject> itemFrames;
    [SerializeField] List<bool> unlocked;
    [SerializeField] List<Button> buttons;
    [SerializeField] KeyCode OpenShopKey;
    [Tooltip("Used when clicking to place a building. This value alters the allowed distance from the center of a cell for the click to be registered")]
    [SerializeField] float distanceFromCenterOfCellAllowance = 1.0f;

    bool inBetween = false;


    [SerializeField] GameObject buildingBig;

    [SerializeField] Button buttonSelect;

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

    bool placing = false;

    private void Start()
    {
        if (buildingsPlaced == null) buildingsPlaced = new List<SC_Building>();
        if (buildingsPlacedLocation == null) buildingsPlacedLocation = new List<Vector3>();

        for (int i = 0; i < buildingsPlaced.Count; i++)
        {
            Vector3 location = new Vector3(buildingsPlacedLocation[i].x, buildingsPlacedLocation[i].y - 0.22f, buildingsPlacedLocation[i].z);
            Instantiate(buildingsPlaced[i], location, Quaternion.identity, buildingParent);
        }
    }

    private void Update()
    {
        //if(Input.GetMouseButtonDown(0) && buildingToPlace != null) CheckForValidGridCell();

        if(placing)
        {
            BuildValid();
        }

        if(!placing && inBetween)
        {
            EventSystem.current.SetSelectedGameObject(buildButton);
            inBetween = false;
        }
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

    public void placeBuilding()
    {
        if(buildingToPlace != null && placing) 
        {
            CheckForValidGridCell();
        }
    }

    public void MoveCusor(Vector2 move)
    {
        if(placing)
        {
            purchaseCursor.Move(move);
        }
    }

    public void SelectBuildingToSpawn()
    {
        //buildmode
        buildingToPlace = selectedBuilding;
        placing = true;
        purchaseCursor.SetBuildingDimensions(selectedBuilding.GetSpriteDims().x, selectedBuilding.GetSpriteDims().y);
        purchaseCursor.gameObject.SetActive(true);
        shopUI.SetActive(false);
        purchaseCursor.GetComponent<SpriteRenderer>().sprite = buildingToPlace.BuildingSprite;
        Cursor.visible = false;
    }

    public void BackToBuildingSelection()
    {
        placing = false;
        purchaseCursor.gameObject.SetActive(false);
        shopUI.SetActive(true);
        Cursor.visible = true;
        buildButton.SetActive(false);
        selectButton.SetActive(false);
        EventSystem.current.SetSelectedGameObject(firstshopButton);
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
        GameManager.Instance.buildMode = false;
        CloseShopUI();
    }

    /// <summary>
    /// Check the locked status of each building and update it's corresponding button
    /// </summary>
    void CheckShopUnlocks()
    {
        int itemsLocked = 0;
        int itemsUnlocked = 0;
        int noMats = 0;
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
                noMats += 1;
                itemsUnlocked += 1;
                buttons[i].interactable = false;
            }
            else if(placed[i] && oneTime[i])
            {
                itemsUnlocked += 1;
                itemsLocked += 1;
                itemFrames[i].GetComponent<SC_BuildButton>().DISABLE();
                buttons[i].interactable = false;
            }
            else 
            {
                itemsUnlocked += 1;
                buttons[i].interactable = true;
                buttons[i].gameObject.SetActive(true);
            }

            if(itemsLocked == itemsUnlocked)
            {
                buttonSelect.interactable = false;
            }
            else if(noMats == itemsUnlocked)
            {
                buttonSelect.interactable = false;
            }
            else
            {
                buttonSelect.interactable = true;
            }
        }
    }

    /// <summary>
    /// Finds grid cell closest to mouse click and check if occupied by another structure or not, used in update to check if current pos is valid
    /// </summary>
    void BuildValid()
    {
        float purchaseX = (purchaseCursor.transform.position.x - buildingToPlace.gameObject.GetComponent<SpriteRenderer>().bounds.extents.x + 0.5f);
        float purchaseY = (purchaseCursor.transform.position.y - buildingToPlace.gameObject.GetComponent<SpriteRenderer>().bounds.extents.y + 0.5f);
        Vector3 purchasePos = new Vector3(purchaseX, purchaseY, purchaseCursor.transform.position.x - buildingToPlace.transform.position.z);
        SC_GridCell closestGridCell = null;
        float closestDistance = float.MaxValue;

        foreach (SC_GridCell cell in grid.GetGrid())
        {
            float distance = Vector2.Distance(cell.transform.position, purchasePos);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestGridCell = cell;
                purchaseCursor.cantPlace();
            }
        }

        if (closestDistance > distanceFromCenterOfCellAllowance)
        {

            return;
        }

        if (!closestGridCell.isOccupied)
        {
            if (buildingToPlace.IsMulticell)
            {
                List<SC_GridCell> usableCells = CheckAdjacentCells(closestGridCell, buildingToPlace.Dimensions);
                if (usableCells.Count == buildingToPlace.Dimensions.x * buildingToPlace.Dimensions.y)
                {
                    purchaseCursor.canPlace();
                }
            }
            else
            {
                purchaseCursor.canPlace();
            }
        }
    }

    /// <summary>
    /// Finds grid cell closest to mouse click and check if occupied by another structure or not
    /// </summary>
    void CheckForValidGridCell()
    {
        float purchaseX = (purchaseCursor.transform.position.x - buildingToPlace.gameObject.GetComponent<SpriteRenderer>().bounds.extents.x + 0.5f);
        float purchaseY = (purchaseCursor.transform.position.y - buildingToPlace.gameObject.GetComponent<SpriteRenderer>().bounds.extents.y + 0.5f);
        Vector3 purchasePos = new Vector3(purchaseX, purchaseY, purchaseCursor.transform.position.x - buildingToPlace.transform.position.z);
        SC_GridCell closestGridCell = null;
        float closestDistance = float.MaxValue;

        foreach (SC_GridCell cell in grid.GetGrid())
        {
            float distance = Vector2.Distance(cell.transform.position, purchasePos);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestGridCell = cell;
            }
        }

        if (closestDistance > distanceFromCenterOfCellAllowance)
        {
            buildingToPlace = null;
            //placing = false;
            //Cursor.visible = true;
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
                    Vector3 spawnedBuildingPos = new Vector3(spawnPosition.x, spawnPosition.y - 0.22f, spawnPosition.z);
                    SC_Building spawnedBuilding = Instantiate(buildingToPlace, spawnedBuildingPos, Quaternion.identity, buildingParent);
                    tentativlyPlacedBuildings.Add(spawnedBuilding);
                    Color buildingColor = spawnedBuilding.GetComponent<SpriteRenderer>().color;
                    spawnedBuilding.GetComponent<SpriteRenderer>().color = new Color(buildingColor.r, buildingColor.g, buildingColor.b, 0.5f);
                    tentativlyPlacedBuildingPrefabs.Add(buildingToPlace);
                    tentativlyPlacedBuildingSpawnPoint.Add(spawnPosition);

                    //ew ew ew ew ew
                    for(int i = 0; i < placed.Count; i++)
                    {
                        SC_BuildButton buttonThis = itemFrames[i].GetComponent<SC_BuildButton>();
                        if(buildingToPlace.buildingName == buttonThis.buildName)
                        {
                            placed[i] = true;
                        }
                    }


                    buildingToPlace = null;
                    foreach(SC_GridCell cell in usableCells)
                    {
                        cell.isOccupied = true;
                        tentativlyAllocatedGridCells.Add(cell);
                    }
                    placing = false;
                    purchaseCursor.gameObject.SetActive(false);
                    buildButton.SetActive(true);
                    selectButton.SetActive(true);
                    Cursor.visible = true;
                    inBetween = true;
                }
            }
            else
            {
                Vector3 spawnedBuildingPos = new Vector3(closestGridCell.transform.position.x, closestGridCell.transform.position.y - 0.22f, closestGridCell.transform.position.z);
                SC_Building spawnedBuilding = Instantiate(buildingToPlace, spawnedBuildingPos, Quaternion.identity, buildingParent);
                tentativlyPlacedBuildings.Add(spawnedBuilding);
                Color buildingColor = spawnedBuilding.GetComponent<SpriteRenderer>().color;
                spawnedBuilding.GetComponent<SpriteRenderer>().color = new Color(buildingColor.r, buildingColor.g, buildingColor.b, 0.5f);
                tentativlyPlacedBuildingPrefabs.Add(buildingToPlace);
                tentativlyPlacedBuildingSpawnPoint.Add(closestGridCell.transform.position);
                buildingToPlace = null;
                closestGridCell.isOccupied = true;
                tentativlyAllocatedGridCells.Add(closestGridCell);
                placing = false;
                purchaseCursor.gameObject.SetActive(false);
                buildButton.SetActive(true);
                selectButton.SetActive(true);
                inBetween = true;
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

    public List<bool> GetPlaced()
    {
        return placed;
    }

    public void SetPlaced(List<bool> placedd)
    {
        placed = placedd;
    }
}
