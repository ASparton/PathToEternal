using System.Collections;
using UnityEngine;

public class DynamicActor : Actor
{
    [Tooltip("Speed when the dynamic actor is moving between cells or rotating.")]
    public int TransitionSpeed = 10;

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
    private IEnumerator MoveToCell(Cell destinationCell)
    {
        inMovement = true;

        Vector3 cellPosition = destinationCell.transform.position;
        Vector3 finalDestination = new Vector3(cellPosition.x, 0f, cellPosition.z);

        while (Vector3.Distance(transform.position, finalDestination) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, finalDestination, Time.deltaTime * TransitionSpeed);
            yield return null;
        }

        transform.position = finalDestination;
        inMovement = false;
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
        isRotating = true;

        while (Mathf.Abs(transform.rotation.eulerAngles.y - finalRotation.eulerAngles.y) > 0.1)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, finalRotation, Time.deltaTime * TransitionSpeed);
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
