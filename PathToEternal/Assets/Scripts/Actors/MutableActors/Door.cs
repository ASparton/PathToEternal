using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Door is a MutableActor.
/// When it is triggered, the door is open, and when it is not, it's closed.
/// </summary>
public class Door : MutableActor
{
    [SerializeField][Tooltip("Set the door open or not at the start of the level.")]
    private bool _isOpen = false;
    public bool IsOpen { get { return _isOpen; } }

    /// <summary>
    /// Open the door.
    /// </summary>
    /// <param name="trigger">The trigger that triggered the action.</param>
    public override void ExecuteTriggerAction(Trigger trigger)
    {
        _isOpen = true;
        StartCoroutine(OpenDoor());
    }

    /// <summary>
    /// Close the door (become a wall).
    /// </summary>
    /// <param name="trigger">The trigger that triggered the action.</param>
    public override void ExecuteUntriggerAction(Trigger trigger)
    {
        _isOpen = false;
        StartCoroutine(CloseDoor());
    }

    /// <summary>
    /// Interpolate the rotation of the door to open it.
    /// </summary>
    /// <returns>Corountine</returns>
    private IEnumerator OpenDoor()
    {
        Vector3 finalPosition = new Vector3(0f, -2f, 0f);

        while (Vector3.Distance(transform.GetChild(0).position, finalPosition) > 0.01f)
        {
            transform.GetChild(0).localPosition = Vector3.Lerp(transform.GetChild(0).localPosition, finalPosition, Time.deltaTime * 10);
            yield return null;
        }

        transform.GetChild(0).localPosition = finalPosition;
    }

    /// <summary>
    /// Interpolate the rotation of the door to close it.
    /// </summary>
    /// <returns>Corountine</returns>
    private IEnumerator CloseDoor()
    {
        Vector3 finalPosition = new Vector3(0f, 0f, 0f);

        while (Vector3.Distance(transform.GetChild(0).position, finalPosition) > 0.01f)
        {
            transform.GetChild(0).localPosition = Vector3.Lerp(transform.GetChild(0).localPosition, finalPosition, Time.deltaTime * 10);
            yield return null;
        }

        transform.GetChild(0).localPosition = finalPosition;
    }
}
