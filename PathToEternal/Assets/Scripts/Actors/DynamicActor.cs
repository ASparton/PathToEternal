using System.Collections;
using UnityEngine;

public class DynamicActor : Actor
{
    [Header("Animations")]
    [Tooltip("Seconds needed to rotate.")]
    public float RotationDuration = 1;

    [Tooltip("Seconds needed to move from one cell to another.")]
    public float CellTransitionDuration = 2;

    [SerializeField][Tooltip("Animator of the dynamic actor -> Can be null.")]
    private Animator AnimationController;

    protected bool inMovement = false;
    protected bool isRotating = false;

    #region Movement

    /// <summary>
    /// Change the current actor cell to the cell with the corresponding given position.
    /// </summary>
    /// <param name="position">The position of the cell to set.</param>
    public void MoveToGridPosition(GridPosition position)
    {
        // Means that it is the same cell, so don't set a new position.
        if (inMovement || position == Cell.GridPosition)
            return;

        Cell newCell = LevelGrid.Instance.GetCell(position);
        if (newCell != null && newCell.Content == null)
        {
            if (newCell.Door != null && !newCell.Door.IsOpen)   // Does not move if there is a closed door on the cell
                return;

            StartCoroutine(MoveToCell(newCell));
        }
    }

    /// <summary>
    /// Interpolate the position of the actor to the given destination cell position.
    /// </summary>
    /// <param name="destinationCell">The destination cell</param>
    /// <returns>Corountine</returns>
    private IEnumerator MoveToCell(Cell destinationCell)
    {
        float yDestination = 0f;
        if (destinationCell.Trigger != null && destinationCell.Trigger.name.Contains("PressurePlate"))
            yDestination = 0.025f;

        Vector3 finalDestination = new Vector3(destinationCell.transform.position.x, yDestination, destinationCell.transform.position.z);
        Vector3 startPosition = transform.position;

        inMovement = true;
        if (AnimationController != null)
            AnimationController.SetBool("isMoving", true);

        float timeElapsed = 0f;
        while (timeElapsed < CellTransitionDuration)
        {
            transform.position = Vector3.Lerp(startPosition, finalDestination, timeElapsed / CellTransitionDuration);

            // Change of cell when close to arrive at destination
            if (Vector3.Distance(transform.position, finalDestination) < 0.5)
            {
                Cell previousCell = Cell;
                Cell = destinationCell;
                previousCell.Content = null;
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = finalDestination;
        inMovement = false;
        if (AnimationController != null)
            AnimationController.SetBool("isMoving", false);
    }

    #endregion
    #region Rotation

    /// <summary>
    /// Make the character smoothly rotate to the given direction.
    /// </summary>
    /// <param name="xDirection">The direction on the x axis (1 -> right & -1 -> left)</param>
    /// <param name="yDirection">The direction on the y axis (1 -> up & -1 -> down)</param>
    protected void LookInDiretion(int xDirection, int yDirection)
    {
        StartCoroutine(RotateOnYTo(GetRotationToDirection(xDirection, yDirection)));
    }

    /// <summary>
    /// Smoothly rotate on the Y axis.
    /// </summary>
    /// <param name="finalRotation">The final rotation to look at.</param>
    /// <returns>Coroutine</returns>
    private IEnumerator RotateOnYTo(Quaternion finalRotation)
    {
        float duration = RotationDuration;
        if (!CameraController.Instance.IsLevelCameraActive())
            duration = RotationDuration * 2;

        Quaternion startRotation = transform.rotation;

        isRotating = true;

        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, finalRotation, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = finalRotation;
        isRotating = false;
    }

    /// <summary>
    /// Determine the rotation of the character depending on the direction given in parameter.
    /// </summary>
    /// <param name="xDirection">The direction on the x axis (1 -> right & -1 -> left)</param>
    /// <param name="yDirection">The direction on the y axis (1 -> up & -1 -> down)</param>
    /// <returns></returns>
    protected Quaternion GetRotationToDirection(int xDirection, int yDirection)
    {
        if (xDirection == 1)
            return Quaternion.AngleAxis(90, Vector3.up);
        if (xDirection == -1)
            return Quaternion.AngleAxis(-90, Vector3.up);
        if (yDirection == -1)
            return Quaternion.AngleAxis(180, Vector3.up);

        return Quaternion.AngleAxis(0, Vector3.up);
    }

    #endregion
}
