using UnityEngine;

/// <summary>
/// An actor represents an object on a grid cell in a level.
/// </summary>
public class Actor : MonoBehaviour
{
    [Header("Grid attributes")]
    [SerializeField][Tooltip("The current cell of the actor.")]
    private Cell _cell = null;
    public Cell Cell
    {
        get { return _cell; }
        
        set // Update the content or trigger of the new cell
        {
            _cell = value;

            if (tag == "Trigger")
                _cell.Trigger = (Trigger)this;
            else if (name.Contains("Door"))
                _cell.Door = (Door)this;
            else
                _cell.Content = this;
        }
    }

    /// <summary>
    /// Assign the cell to the actor if it was assigned in the editor. Print a warning otherwise.
    /// </summary>
    private void Start()
    {
        if (_cell == null)
        {
            if (tag != "Player")
                print("Warning: This actor " + name + " does not have an assigned cell.");
        }
        else
        {
            // Set the actor position
            Vector3 cellPosition = _cell.transform.position;
            transform.position = new Vector3(cellPosition.x, transform.position.y, cellPosition.z);
        }

        Cell = _cell;
    }
}
