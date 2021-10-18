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
        if (!inMovement && !isRotating)
        {
            int xDirection = GetXDirection(), yDirection = GetYDirection();

            // If any direction is pointed
            if (xDirection != 0 || yDirection != 0)
            {
                if (transform.rotation != GetRotationToDirection(xDirection, yDirection))
                    LookInDiretion(xDirection, yDirection);
                else
                {
                    GridPosition pointedPosition = new GridPosition(Cell.GridPosition.x, Cell.GridPosition.y);

                    // Points on the horizontal axis
                    if (xDirection != 0 && yDirection == 0)
                        pointedPosition = new GridPosition(Cell.GridPosition.x + xDirection, Cell.GridPosition.y);
                    // Or points on the vertical axis
                    else if (xDirection == 0 && yDirection != 0)
                        pointedPosition = new GridPosition(Cell.GridPosition.x, Cell.GridPosition.y + yDirection);
                    // else -> diagonals are forbidden

                    Cell pointedCell = LevelGrid.Instance.GetCell(pointedPosition);
                    if (pointedCell != null)
                        ExecuteAction(pointedCell);
                }
            }
        }
    }

    /// <summary>
    /// Execute an action depending on the the next cell's content.
    /// </summary>
    /// <param name="pointedCell">The position pointed by the player</param>
    private void ExecuteAction(Cell pointedCell)
    {
        // If there's no content in the pointed cell, the player moves to it
        switch (pointedCell.Content)
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

    /// <summary>
    /// Determine the direction the player is pointing to on the X axis depending on the active camera and his inputs.
    /// </summary>
    /// <returns>The direction the player is pointing to on the X axis depending on the active camera and his inputs.</returns>
    private int GetXDirection()
    {
        int xDirection = 0;

        if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.A))
        {
            if (CameraController.Instance.IsLevelCameraActive())
                xDirection = -1;
            else
                xDirection = GetXDirectionOnKey(KeyCode.Q);
        }

        else if (Input.GetKeyUp(KeyCode.D))
        {
            if (CameraController.Instance.IsLevelCameraActive())
                xDirection = 1;
            else
                xDirection = GetXDirectionOnKey(KeyCode.D);
        }

        else if (Input.GetKeyUp(KeyCode.S) && !CameraController.Instance.IsLevelCameraActive())
            xDirection = GetXDirectionOnKey(KeyCode.S);

        else if ((Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.W)) && !CameraController.Instance.IsLevelCameraActive())
            xDirection = GetXDirectionOnKey(KeyCode.Z);

        return xDirection;
    }

    /// <summary>
    /// Determine the direction the player is pointing to on the Y axis depending on the active camera and his inputs.
    /// </summary>
    /// <returns>The direction the player is pointing to on the Y axis depending on the active camera and his inputs.</returns>
    private int GetYDirection()
    {
        int yDirection = 0;

        if ((Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.A)) && !CameraController.Instance.IsLevelCameraActive())
            yDirection = GetYDirectionOnKey(KeyCode.Q);

        else if (Input.GetKeyUp(KeyCode.D) && !CameraController.Instance.IsLevelCameraActive())
            yDirection = GetYDirectionOnKey(KeyCode.D);

        else if (Input.GetKeyUp(KeyCode.S))
        {
            if (CameraController.Instance.IsLevelCameraActive())
                yDirection = -1;
            else
                yDirection = GetYDirectionOnKey(KeyCode.S);
        }

        else if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.W))
        {
            if (CameraController.Instance.IsLevelCameraActive())
                yDirection = 1;
            else
                yDirection = GetYDirectionOnKey(KeyCode.Z);
        }

        return yDirection;
    }

    /// <summary>
    /// Determine the direction on the x axis depending on the key pressed and the actual rotation of the player.
    /// </summary>
    /// <param name="keyPressed">The key pressed by the player</param>
    /// <returns>The direction on the x axis depending on the key pressed and the actual rotation of the player.</returns>
    private int GetXDirectionOnKey(KeyCode keyPressed)
    {
        if (transform.rotation == Quaternion.AngleAxis(90, Vector3.up))
        {
            switch (keyPressed)
            {
                case KeyCode.Z:
                    return 1;
                case KeyCode.S:
                    return -1;
                default:
                    return 0;
            }
        }
        else if (transform.rotation == Quaternion.AngleAxis(-90, Vector3.up))
        {
            switch (keyPressed)
            {
                case KeyCode.Z:
                    return -1;
                case KeyCode.S:
                    return 1;
                default:
                    return 0;
            }
        }
        else if (transform.rotation == Quaternion.AngleAxis(0, Vector3.up))
        {
            switch (keyPressed)
            {
                case KeyCode.Q:
                    return -1;
                case KeyCode.D:
                    return 1;
                default:
                    return 0;
            }
        }
        else if (transform.rotation == Quaternion.AngleAxis(180, Vector3.up))
        {
            switch (keyPressed)
            {
                case KeyCode.Q:
                    return 1;
                case KeyCode.D:
                    return -1;
                default:
                    return 0;
            }
        }
        else
            return 0;
    }

    /// <summary>
    /// Determine the direction on the y axis depending on the key pressed and the actual rotation of the player.
    /// </summary>
    /// <param name="keyPressed">The key pressed by the player</param>
    /// <returns>The direction on the y axis depending on the key pressed and the actual rotation of the player.</returns>
    private int GetYDirectionOnKey(KeyCode keyPressed)
    {
        if (transform.rotation == Quaternion.AngleAxis(90, Vector3.up))
        {
            switch (keyPressed)
            {
                case KeyCode.Q:
                    return 1;
                case KeyCode.D:
                    return -1;
                default:
                    return 0;
            }
        }
        else if (transform.rotation == Quaternion.AngleAxis(-90, Vector3.up))
        {
            switch (keyPressed)
            {
                case KeyCode.Q:
                    return -1;
                case KeyCode.D:
                    return 1;
                default:
                    return 0;
            }
        }
        else if (transform.rotation == Quaternion.AngleAxis(0, Vector3.up))
        {
            switch (keyPressed)
            {
                case KeyCode.Z:
                    return 1;
                case KeyCode.S:
                    return -1;
                default:
                    return 0;
            }
        }
        else if (transform.rotation == Quaternion.AngleAxis(180, Vector3.up))
        {
            switch (keyPressed)
            {
                case KeyCode.Z:
                    return -1;
                case KeyCode.S:
                    return 1;
                default:
                    return 0;
            }
        }
        else
            return 0;
    }
}
