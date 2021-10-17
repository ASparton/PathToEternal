using System.Collections;
using UnityEngine;

public class DynamicActor : Actor
{
    private bool inMovement;

    /// <summary>
    /// At the first frame, the actor does not move.
    /// </summary>
    private void Start()
    {
        inMovement = false;
    }

    /// <summary>
    /// Change the current actor cell to the cell with the corresponding given position.
    /// </summary>
    /// <param name="position">The position of the cell to set.</param>
    public void MoveToGridPosition(GridPosition position)
    {
        // Means that it is the same cell, so don't set a new position.
        if (inMovement || position == Cell.GridPosition)
            return;

        Cell newCell = LevelGrid.instance.GetCell(position);
        if (newCell != null && newCell.Content == null)
        {
            StartCoroutine(InterpolateToCell(newCell));

            // Change of cell when fully arrived at destination
            Cell previousCell = Cell;
            Cell = newCell;
            previousCell.Content = null;
        }
    }

    /// <summary>
    /// Interpolate the position of the actor to the given destination cell position.
    /// </summary>
    /// <param name="destinationCell">The destination cell</param>
    /// <returns>Corountine</returns>
    private IEnumerator InterpolateToCell(Cell destinationCell)
    {
        inMovement = true;

        Vector3 cellPosition = destinationCell.transform.position;
        Vector3 finalDestination = new Vector3(cellPosition.x, transform.localScale.y / 2, cellPosition.z);

        while (Vector3.Distance(transform.position, finalDestination) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, finalDestination, Time.deltaTime * 10);
            yield return null;
        }

        transform.position = finalDestination;
        inMovement = false;
    }
}
