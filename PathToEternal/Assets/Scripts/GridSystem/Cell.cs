using UnityEngine;

/// <summary>
/// Represents a location on a grid during a level.
/// </summary>
public class Cell : MonoBehaviour
{
    public GridPosition GridPosition;     // The position of the cell on the grid
    public Actor Content { get; set; }  // The possible actor that the cell can contains

    /// <summary>
    /// Set and fix the grid position of the cell.
    /// </summary>
    private void Awake() => GridPosition = new GridPosition((int)transform.localPosition.x, (int)transform.localPosition.z);
}
