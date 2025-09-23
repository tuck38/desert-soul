using System.Collections.Generic;
using UnityEngine;

public enum GridStartLocation
{
    UPPERLEFT,
    UPPERRIGHT,
    LOWERLEFT,
    LOWERRIGHT
}

public class SC_GridMaker : MonoBehaviour
{
    [Header("Grid Rules")]
    [SerializeField] GameObject gridCellPrefab;
    [SerializeField] Vector2 gridCellSize;
    [SerializeField] Vector2 spaceBetweenCells;
    [SerializeField] Vector2Int gridDimensions;
    [SerializeField] Vector2 gridStartPosition;
    [SerializeField] GridStartLocation gridStartCorner;

    [Header("Current Grid")]
    [SerializeField] List<SC_GridCell> grid;

    public Vector2Int GridDimensions { get => gridDimensions; }
    public GridStartLocation GridStartCorner { get => gridStartCorner; }

    /// <summary>
    /// Spawn in grid cells based on class parameters
    /// </summary>
    [ContextMenu("Build Grid")]
    void BuildGrid()
    {
        ClearGrid();
        
        float x, y;
        int xIndex, yIndex = 0;
        bool xCriteria, yCriteria;

        yCriteria = UpdateYCritera(yIndex);

        for (yIndex = 0; yCriteria;)
        {
            y = gridStartPosition.y + gridCellSize.y * yIndex + spaceBetweenCells.y * yIndex;

            xIndex = 0;
            xCriteria = UpdateXCritera(xIndex);

            for (xIndex = 0; xCriteria;)
            {
                x = gridStartPosition.x + gridCellSize.x * xIndex + spaceBetweenCells.x * xIndex;
                grid.Add(Instantiate(gridCellPrefab, new Vector2(x,y), Quaternion.identity, transform).GetComponent<SC_GridCell>());

                if (gridStartCorner == GridStartLocation.LOWERLEFT || gridStartCorner == GridStartLocation.UPPERLEFT) xIndex++;
                else if (gridStartCorner == GridStartLocation.LOWERRIGHT || gridStartCorner == GridStartLocation.UPPERRIGHT) xIndex--;

                xCriteria = UpdateXCritera(xIndex);
            }

            if (gridStartCorner == GridStartLocation.LOWERLEFT || gridStartCorner == GridStartLocation.LOWERRIGHT) yIndex++;
            else if (gridStartCorner == GridStartLocation.UPPERLEFT || gridStartCorner == GridStartLocation.UPPERRIGHT) yIndex--;

            yCriteria = UpdateYCritera(yIndex);
        }
    }

    bool UpdateXCritera(int xIndex) 
    {
        if (gridStartCorner == GridStartLocation.LOWERLEFT || gridStartCorner == GridStartLocation.UPPERLEFT) return xIndex < gridDimensions.x;
        else return xIndex > gridDimensions.x * -1;
    }

    bool UpdateYCritera(int yIndex) 
    {
        if (gridStartCorner == GridStartLocation.LOWERLEFT || gridStartCorner == GridStartLocation.LOWERRIGHT) return yIndex < gridDimensions.y;
        else return yIndex > gridDimensions.y * -1;
    }

    /// <summary>
    /// Remove and destroy all grid cells
    /// </summary>
    [ContextMenu("Clear Grid")]
    void ClearGrid()
    {
        if (grid.Count == 0) return;

        for(int i = grid.Count-1; i >= 0; i--)
        {
            DestroyImmediate(grid[i].gameObject);
        }
        grid.Clear();
    }

    /// <summary>
    /// Returns List structure with all spawned grid cells
    /// </summary>
    /// <returns></returns>
    public List<SC_GridCell> GetGrid()
    {
        return grid;
    }
}
