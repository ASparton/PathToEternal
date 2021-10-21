using System.Collections;
using UnityEngine;

public class Player : Character
{
    // Delegate indicating where the player moved every time he did
    public delegate void PlayerMoved(Cell newCell);
    public static event PlayerMoved PlayerMovedEvent;

    [Header("Animations")]
    [SerializeField][Tooltip("Duration of the spawn animation.")]
    private float SpawnFallDuration = 2f;

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
        if (pointedCell.Content != null)
        {
            switch (pointedCell.Content.tag)
            {
                // Futur actions to implement here

                default: // Otherwise the player tries to move to the cell.
                    HandleMovement(pointedCell);
                    break;
            }
        }
        else // If the cell has no content, the player moves to it
            HandleMovement(pointedCell);
    }

    /// <summary>
    /// Tries to move to the given cell. If he can it notifies that he moved, otherwise it starts the "Can't move" animation.
    /// </summary>
    /// <param name="destinationCell">The cell to try to move to.</param>
    private void HandleMovement(Cell destinationCell)
    {
        if (MoveToGridPosition(destinationCell.GridPosition))
            PlayerMovedEvent.Invoke(destinationCell);
        else
        {
            if (!inMovement && !isRotating)
                StartCoroutine(PlayerCantMoveAnimation());
        }
    }

    /// <summary>
    /// Activate or deactivate the player weapons.
    /// </summary>
    /// <param name="active">True to activate, false to deactivate.</param>
    public void SetWeaponsActive(bool active)
    {
        gameObject.transform.GetChild(1).GetChild(0).Find("Shield").gameObject.SetActive(active);
        gameObject.transform.GetChild(1).GetChild(0).Find("Weapon").gameObject.SetActive(active);
    }

    #region Player specific animations

    /// <summary>
    /// Make the player fall down and rotate to the entry cell position given.
    /// </summary>
    /// <param name="entryCellPosition">The entry cell position.</param>
    public void Spawn(Vector3 entryCellPosition)
    {
        StartCoroutine(PlayerSpawnAnimation(entryCellPosition));
    }

    /// <summary>
    /// Make the player rotate and decrease its scale to 0 during the end animation.
    /// </summary>
    /// <param name="endAnimationDuration">The duration of the animation</param>
    /// <param name="usedToSpawn">True if the coroutine in used when spawning the player.</param>
    /// <returns>Coroutine</returns>
    public IEnumerator PlayerDizzyAnimation(float endAnimationDuration)
    {
        // Start animation
        AnimationController.SetBool("isDizzy", true);

        float timeElapsed = 0f;
        while (timeElapsed < endAnimationDuration)
        {
            Vector3 eulerAngles = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(new Vector3(eulerAngles.x, eulerAngles.y + 1f, eulerAngles.z));

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Stop animation
        AnimationController.SetBool("isDizzy", false);
    }

    /// <summary>
    /// Make the player fall down to the destination cell position given.
    /// </summary>
    /// <param name="destinationCellPosition">The destination cell position</param>
    /// <returns>Coroutine</returns>
    private IEnumerator PlayerSpawnAnimation(Vector3 destinationCellPosition)
    {
        // Start animation
        AnimationController.SetBool("isDizzy", true);

        Vector3 startPosition = new Vector3(destinationCellPosition.x, destinationCellPosition.y + 3f, destinationCellPosition.z);

        Quaternion startRotation = Quaternion.AngleAxis(-180f, Vector3.up);
        Quaternion finalRotation = Quaternion.AngleAxis(0, Vector3.up);

        float timeElapsed = 0f;
        while (timeElapsed < SpawnFallDuration)
        {
            transform.position = Vector3.Lerp(startPosition, destinationCellPosition, timeElapsed / SpawnFallDuration);
            transform.rotation = Quaternion.Lerp(startRotation, finalRotation, timeElapsed / SpawnFallDuration);    
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = destinationCellPosition;
        transform.rotation = finalRotation;

        // Stop animation
        AnimationController.SetBool("isDizzy", false);
    }

    /// <summary>
    /// Play the shaking head no animations for 1 second.
    /// </summary>
    /// <returns>Coroutine</returns>
    private IEnumerator PlayerCantMoveAnimation()
    {
        // Start animation
        AnimationController.SetBool("cantMove", true);
        inMovement = true;
        SetWeaponsActive(false);

        yield return new WaitForSeconds(1f);

        // Stop animation
        AnimationController.SetBool("cantMove", false);
        inMovement = false;
        SetWeaponsActive(true);
    }

    #endregion
    #region Direction choice management

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

    #endregion
}
