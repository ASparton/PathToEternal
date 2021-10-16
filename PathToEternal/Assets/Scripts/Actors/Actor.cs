using UnityEngine;

/// <summary>
/// An actor represents an object on a grid cell in a level.
/// </summary>
[RequireComponent(typeof(Cell))]
public class Actor : MonoBehaviour
{
    [Tooltip("The current cell of the actor.")]
    public Cell cell = null;

    /// <summary>
    /// Position the actor at the center of the cell.
    /// </summary>
    void Start()
    {
        if (cell == null)
        {
            print("Actor " + name + " does not have an assigned cell.");
            return;
        }

        Vector3 cellPosition = cell.transform.position;
        transform.position = new Vector3(cellPosition.x, transform.localScale.y / 2, cellPosition.z);
    }
}
