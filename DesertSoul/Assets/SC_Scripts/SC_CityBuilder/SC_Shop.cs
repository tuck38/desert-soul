using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SC_Shop : MonoBehaviour
{
    [SerializeField] GameObject shopUIParent;
    [SerializeField] GameObject enableShopButtonParent;
    [SerializeField] Transform buildingParent;
    [SerializeField] SC_MouseTracker purchaseCursor;
    [SerializeField] SC_GridMaker grid;
    [SerializeField] List<SC_Building> buildings;
    [SerializeField] List<Button> buttons;

    private SC_Building buildingToPlace;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && buildingToPlace != null) CheckForValidGridCell();
    }

    /// <summary>
    /// Enables player to place input building onto map
    /// </summary>
    /// <param name="building"></param>
    public void SelectBuildingToSpawn(SC_Building building)
    {
        SC_ResourceTestScript.OnResourcesAmountChanged(building.MaterialCost.x * -1, building.MaterialCost.y * -1);
        buildingToPlace = building;
        purchaseCursor.gameObject.SetActive(true);
        purchaseCursor.GetComponent<SpriteRenderer>().sprite = building.BuildingSprite;
        Cursor.visible = false;
    }

    /// <summary>
    /// Makes Menu UI visible, removing the activation button from view
    /// </summary>
    public void OpenShopUI()
    {
        CheckShopUnlocks();
        shopUIParent.SetActive(true);
        enableShopButtonParent.SetActive(false);
        grid.gameObject.SetActive(true);
    }

    /// <summary>
    /// Makes Menu UI invisible, bringing the activation button back into view
    /// </summary>
    public void CloseShopUI()
    {
        shopUIParent.SetActive(false);
        enableShopButtonParent.SetActive(true);
        grid.gameObject.SetActive(false);
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

            if (!buildings[i].Unlocked) buttons[i].interactable = false;
            else if (buildings[i].MaterialCost.x > SC_ResourceTestScript.amountOfBrown || buildings[i].MaterialCost.y > SC_ResourceTestScript.amountOfPurple) buttons[i].interactable = false;
            else buttons[i].interactable = true;
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

        if (closestDistance > 1)
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
                    Instantiate(buildingToPlace, closestGridCell.transform.position, Quaternion.identity, buildingParent);
                    buildingToPlace = null;
                    foreach(SC_GridCell cell in usableCells)
                    {
                        cell.isOccupied = true;
                    }
                    purchaseCursor.gameObject.SetActive(false);
                    Cursor.visible = true;
                }
            }
            else
            {
                Instantiate(buildingToPlace, closestGridCell.transform.position, Quaternion.identity, buildingParent);
                buildingToPlace = null;
                closestGridCell.isOccupied = true;
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
                int nextIndex = selectedCellIndex + (xIndex * gridArrayMovement.x) + (yIndex * grid.GridDimensions.y * gridArrayMovement.y);
                int modDiff = (previousIndex % grid.GridDimensions.x) - (nextIndex % grid.GridDimensions.x);
                if (nextIndex >= 0 && nextIndex < grid.GetGrid().Count)
                {
                    if (xIndex == 0 || (Math.Abs(modDiff) == 1 && xIndex != 0))
                    {
                        if (!grid.GetGrid()[nextIndex].isOccupied)
                        {
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
}
