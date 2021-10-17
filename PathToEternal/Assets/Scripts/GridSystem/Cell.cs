using UnityEngine;

/// <summary>
/// Represents a location on a grid during a level.
/// </summary>
public class Cell : MonoBehaviour
{
    public GridPosition GridPosition;     // The position of the cell on the grid

    private DynamicActor _dynamicActor;
    public DynamicActor DynamicActor    // The possible actor that the cell can contains
    { 
        get { return _dynamicActor; }
        set // Also set the Trigger as triggered if the content tag is contained in the Trigger matching tags
        {
            _dynamicActor = value;
            if (_dynamicActor != null)
            {
                if (Trigger != null && !Trigger.IsTriggered && Trigger.MatchingTags.Contains(_dynamicActor.tag))
                    Trigger.IsTriggered = true;
            }
            else
            {
                if (Trigger != null && !Trigger.KeepTriggered)
                    Trigger.IsTriggered = false;
            }
        }
    }

    public Trigger Trigger { get; set; }    // The possible trigger that the cell can contains
    public Door Door { get; set; }          // The possible door the cell can contains


    /// <summary>
    /// Set and fix the grid position of the cell.
    /// </summary>
    private void Awake() => GridPosition = new GridPosition((int)transform.localPosition.x, (int)transform.localPosition.z);
}
