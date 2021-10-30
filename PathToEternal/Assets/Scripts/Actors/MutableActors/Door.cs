using System.Collections;
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
        StartCoroutine(PlayActionCinematic());
        StartCoroutine(SetDoorState(true));
    }

    /// <summary>
    /// Close the door (become a wall).
    /// </summary>
    /// <param name="trigger">The trigger that triggered the action.</param>
    public override void ExecuteUntriggerAction(Trigger trigger)
    {
        _isOpen = false;
        StartCoroutine(PlayActionCinematic());
        StartCoroutine(SetDoorState(false));
    }

    /// <summary>
    /// Interpolate the rotation of the door to open it.
    /// </summary>
    /// <returns>Corountine</returns>
    private IEnumerator SetDoorState(bool open)
    {
        Vector3 startPosition = transform.GetChild(0).localPosition;
        Vector3 finalPosition;
        if (open)
            finalPosition = new Vector3(0f, -2f, 0f);
        else
            finalPosition = new Vector3(0f, 0f, 0f);

        float timeElapsed = -1f;
        while (timeElapsed < _actionAnimationDuration)
        {
            if (timeElapsed >= 0f)
                transform.GetChild(0).localPosition = Vector3.Lerp(startPosition, finalPosition, timeElapsed / _actionAnimationDuration);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.GetChild(0).localPosition = finalPosition;
    }

    /// <summary>
    /// Activate the action camera the time of the action cinematic.
    /// </summary>
    /// <returns>Coroutine</returns>
    private IEnumerator PlayActionCinematic()
    {
        // Activate action camera
        GenericCamera previousCamera = CameraController.Instance.GetCurrentCamera();
        CameraController.Instance.ActivateCamera(ActionCamera);
        LevelGrid.Instance.SetGameInputsEnabled(false);

        yield return new WaitForSecondsRealtime(_actionAnimationDuration + 2f);

        // Deactivate the action camera
        CameraController.Instance.ActivateCamera(previousCamera);
        LevelGrid.Instance.SetGameInputsEnabled(true);
    }
}
