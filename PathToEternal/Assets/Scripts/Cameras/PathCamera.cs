using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a camera that can follow a list of locations:
/// - Interpolate its position to the next point on the list
/// - Interpolate its position to the previous point on the list
/// </summary>
public class PathCamera : GenericCamera
{
    #region Editor location & camera lists

    [SerializeField][Tooltip("All the locations of the camera.")]
    private List<Vector3> locationList;

    [SerializeField][Tooltip("All the locations of the camera.")]
    private List<Quaternion> rotationList;

    #endregion

    [SerializeField][Tooltip("The duration in second of the camera transitions.")]
    private float transitionSpeed = 1;

    private LinkedList<Vector3> locations;
    private LinkedListNode<Vector3> currentLocation;

    private LinkedList<Quaternion> rotations;
    private LinkedListNode<Quaternion> currentRotation;

    /// <summary>
    /// Linked lists setup.
    /// </summary>
    private void Awake()
    {
        locations = new LinkedList<Vector3>();
        rotations = new LinkedList<Quaternion>();

        foreach (Vector3 location in locationList)
            locations.AddLast(location);

        foreach (Quaternion rotation in rotationList)
            rotations.AddLast(rotation);
    }

    /// <summary>
    /// Initialise the current and first location and rotation of the camera.
    /// </summary>
    private void Start()
    {
        currentLocation = locations.First;
        currentRotation = rotations.First;

        transform.position = currentLocation.Value;
        transform.rotation = currentRotation.Value;
    }

    /// <summary>
    /// Listen to user inputs and prepare for changing of point of view.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            currentLocation = currentLocation.Next;
            if (currentLocation == null)
                currentLocation = locations.First;

            currentRotation = currentRotation.Next;
            if (currentRotation == null)
                currentRotation = rotations.First;
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            currentLocation = currentLocation.Previous;
            if (currentLocation == null)
                currentLocation = locations.Last;

            currentRotation = currentRotation.Previous;
            if (currentRotation == null)
                currentRotation = rotations.Last;
        }
    }

    /// <summary>
    /// Interpolate to new camera location and rotation if it changed.
    /// </summary>
    private void LateUpdate()
    {
        if (transform.position != currentLocation.Value)
        {
            transform.position = Vector3.Lerp(transform.position, currentLocation.Value, Time.deltaTime * transitionSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, currentRotation.Value, Time.deltaTime * transitionSpeed);
        }
    }
}
