using System.Collections;
using UnityEngine;

public class Player : Character
{
    // Delegate indicating where the player moved every time he did
    public delegate void PlayerMoved(GridPosition newPosition);
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
            GridPosition nextPosition = new GridPosition(Cell.GridPosition.x, Cell.GridPosition.y);

            // Move on the horizontal axis
            if (xDirection != 0 && yDirection == 0)
                nextPosition = new GridPosition(Cell.GridPosition.x + xDirection, Cell.GridPosition.y);
            // Or move on the vertical axis
            else if (xDirection == 0 && yDirection != 0)
                nextPosition = new GridPosition(Cell.GridPosition.x, Cell.GridPosition.y + yDirection);
            // else -> a diagonal are forbidden

            SetGridPosition(nextPosition);
            if (PlayerMovedEvent != null)
                PlayerMovedEvent.Invoke(nextPosition);
        }
    }
}
