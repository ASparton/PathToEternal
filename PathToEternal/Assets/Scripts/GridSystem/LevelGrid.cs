using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the level grid, composed of cells.
/// </summary>
public class LevelGrid : MonoBehaviour
{
    static public LevelGrid instance;

    [SerializeField]
    private List<Cell> cells = new List<Cell>();

    /// <summary>
    /// Assure the singleton pattern.
    /// </summary>
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }

    /// <summary>
    /// Try to find the cell which has the same position as the one given and returns it.
    /// </summary>
    /// <param name="cellPosition">The position of the cell in the grid.</param>
    /// <returns>The cell that has the corresponding position and null if not found.</returns>
    public Cell GetCell(GridPosition cellPosition)
    {
        foreach (Cell cell in cells)
        {
            if (cell.GridPosition.Equals(cellPosition))
                return cell;
        }
        return null;
    }
}
