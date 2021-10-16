using UnityEngine;

/// <summary>
/// An actor represents an object on a grid cell in a level.
/// </summary>
public class Actor : MonoBehaviour
{
    [SerializeField][Tooltip("The current cell of the actor.")]
    private Cell _cell = null;
    public Cell Cell
    {
        get { return _cell; }
        
        set // Also change the current position of the actor
        {
            _cell = value;

            Vector3 cellPosition = _cell.transform.position;
            transform.position = new Vector3(cellPosition.x, transform.localScale.y / 2, cellPosition.z);
        }
    }

    /// <summary>
    /// Position the actor at the center of the cell.
    /// </summary>
    protected virtual void Start()
    {
        if (_cell == null)
        {
            print("Actor " + name + " does not have an assigned cell.");
            return;
        }

        Cell = _cell;
    }
}
