using System.Collections;
using UnityEngine;

public class Player : Character
{
    // Delegate indicating where the player moved every time he did
    public delegate void PlayerMoved(Cell newCell);
    public static event PlayerMoved PlayerMovedEvent;

    /// <summary>
    ///  Listen and react to the user inputs.
    /// </summary>
    private void Update()
    {
        int xDirection = 0;
        if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.A))
            xDirection = -1;
        else if (Input.GetKeyUp(KeyCode.D))
            xDirection = 1;

        int yDirection = 0;
        if (Input.GetKeyUp(KeyCode.S))
           yDirection = -1;
        else if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.W))
            yDirection = 1;

        // If any movement input is detected
        if (xDirection != 0 || yDirection != 0)
        {
            GridPosition pointedPosition = new GridPosition(Cell.GridPosition.x, Cell.GridPosition.y);

            // Points on the horizontal axis
            if (xDirection != 0 && yDirection == 0)
                pointedPosition = new GridPosition(Cell.GridPosition.x + xDirection, Cell.GridPosition.y);
            // Or points on the vertical axis
            else if (xDirection == 0 && yDirection != 0)
                pointedPosition = new GridPosition(Cell.GridPosition.x, Cell.GridPosition.y + yDirection);
            // else -> diagonals are forbidden

            Cell pointedCell = LevelGrid.instance.GetCell(pointedPosition);
            if (pointedCell != null)
                ExecuteAction(pointedCell);
        }
    }

    /// <summary>
    /// Execute an action depending on the the next cell's content.
    /// </summary>
    /// <param name="pointedCell">The position pointed by the player</param>
    private void ExecuteAction(Cell pointedCell)
    {
        // If there's no content in the pointed cell, the player moves to it
        switch (pointedCell.DynamicActor)
        {
            // More actions to implement in the future

            // If there is no content in the pointed cell the player moves to it.
            default:
                MoveToGridPosition(pointedCell.GridPosition);
                if (PlayerMovedEvent != null)
                    PlayerMovedEvent.Invoke(pointedCell);
                break;
        }
    }
}
