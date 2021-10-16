using UnityEngine;

/// <summary>
/// Represents a location on a grid during a level.
/// </summary>
public class Cell : MonoBehaviour
{
    public GridPosition GridPosition;     // The position of the cell on the grid
    //readonly Actor actor; // The possible actor that it can contains

    /// <summary>
    /// Set and fix the grid position of the cell.
    /// </summary>
    public void Awake()
    {
        GridPosition = new GridPosition((int)transform.localPosition.x, (int)transform.localPosition.y);
    }
}
