using System.Collections;
using UnityEngine;

public class DynamicActor : Actor
{
    [Header("Animations")]
    [SerializeField][Tooltip("Seconds needed to rotate.")]
    private float _rotationDuration = 1f;
    public float RotationDuration { get { return _rotationDuration; } }

    [SerializeField][Tooltip("Seconds needed to move from one cell to another.")]
    private float _cellTransitionDuration = 2f;
    public float CellTransitionDuration { get { return _cellTransitionDuration; } }

    [SerializeField][Tooltip("Animator of the dynamic actor -> Can be null.")]
    protected Animator AnimationController;

    protected bool inMovement = false;
    protected bool isRotating = false;

    #region Movement

    /// <summary>
    /// Try to make the actor move in the given direction.
    /// </summary>
    /// <param name="direction">The direction of the movement.</param>
    /// <returns>True if the character is able to move, false if he can't.</returns>
    public bool MoveToGridPosition(Direction direction)
    {
        // Means that it is the same cell, so don't set a new position.
        if (inMovement)
            return false;

        Cell nextCell = LevelGrid.Instance.GetCell(Cell.GridPosition, direction);
        if (nextCell != null)
        {
            if (nextCell.Content != null && nextCell.Content.tag == "Wall")
                return false;
            if (nextCell.Door != null && !nextCell.Door.IsOpen)   // Does not move if there is a closed door on the cell
                return false;
            if (nextCell.Content != null && nextCell.Content.tag == "Crate")
            {
                DynamicActor nextCellCrate = (DynamicActor)nextCell.Content;
                if (nextCellCrate.MoveToGridPosition(direction))
                {
                    StartCoroutine(MoveToCell(nextCell));
                    return true;
                }
                else
                    return false;   // There is a wall or a closed door behind the crate
            }

            StartCoroutine(MoveToCell(nextCell));
            return true;
        }
        return false;
    }

    /// <summary>
    /// Free the content of the actual cell and set the new cell of this dynamic character.
    /// </summary>
    /// <param name="newCell">The new cell to assign to the dynamic character.</param>
    private void ChangeCell(Cell newCell)
    {
        Cell previousCell = Cell;
        Cell = newCell;
        previousCell.Content = null;
    }

    /// <summary>
    /// Interpolate the position of the actor to the given destination cell position.
    /// </summary>
    /// <param name="destinationCell">The destination cell</param>
    /// <returns>Corountine</returns>
    private IEnumerator MoveToCell(Cell destinationCell)
    {
        Vector3 finalDestination = new Vector3(GetXDestination(destinationCell), GetYDestination(destinationCell), destinationCell.transform.position.z);
        Vector3 startPosition = transform.position;

        inMovement = true;
        if (AnimationController != null)
            AnimationController.SetBool("isMoving", true);

        bool cellChanged = false;

        float timeElapsed = 0f;
        while (timeElapsed < _cellTransitionDuration)
        {
            transform.position = Vector3.Lerp(startPosition, finalDestination, timeElapsed / _cellTransitionDuration);

            // Cell changing
            if (Vector3.Distance(transform.position, finalDestination) < 0.5f && !cellChanged)
            {
                cellChanged = true;
                ChangeCell(destinationCell);
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = finalDestination;
        inMovement = false;
        if (AnimationController != null)
            AnimationController.SetBool("isMoving", false);
    }

    /// <summary>
    /// Determine the position on the y axis on the next cell.
    /// </summary>
    /// <param name="destinationCell">The cell to move to.</param>
    /// <returns>The position on the y axis on the next cell.</returns>
    private float GetYDestination(Cell destinationCell)
    {
        float yDestination = destinationCell.transform.position.y;
        if (tag == "Crate")
            yDestination += transform.localScale.y / 2;
        else if (destinationCell.Trigger != null && destinationCell.Trigger.name.Contains("PressurePlate"))
            yDestination += 0.025f;
        else if (destinationCell.Content != null && destinationCell.Content.tag == "Exit")
            yDestination += 0.35f;

        return yDestination;
    }

    /// <summary>
    /// Determine the position on the x axis on the next cell.
    /// </summary>
    /// <param name="destinationCell">The cell to move to.</param>
    /// <returns>The position on the x axis on the next cell.</returns>
    private float GetXDestination(Cell destinationCell)
    {
        float xDestination = destinationCell.transform.position.x;

        if (destinationCell.Content != null && destinationCell.Content.tag == "Exit")
            xDestination -= 0.05f;

        return xDestination;
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
        float duration = _rotationDuration;
        if (!CameraController.Instance.IsLevelCameraActive())
            duration = _rotationDuration * 2;

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
