using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a camera that can follow a list of locations:
/// - Interpolate its position to the next point on the list
/// - Interpolate its position to the previous point on the list
/// </summary>
public class PathCamera : MonoBehaviour
{
    #region Editor fields

    [SerializeField][Tooltip("All the locations of the camera.")]
    private List<Vector3> locationList;

    [SerializeField][Tooltip("All the locations of the camera.")]
    private List<Quaternion> rotationList;

    #endregion

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
        UpdateCamera(currentLocation.Value, currentRotation.Value);
    }

    /// <summary>
    /// Listen to user inputs and change the camera location and rotation.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            currentLocation = currentLocation.Next;
            if (currentLocation == null)
                currentLocation = locations.First;

            currentRotation = currentRotation.Next;
            if (currentRotation == null)
                currentRotation = rotations.First;

            UpdateCamera(currentLocation.Value, currentRotation.Value);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            currentLocation = currentLocation.Previous;
            if (currentLocation == null)
                currentLocation = locations.Last;

            currentRotation = currentRotation.Previous;
            if (currentRotation == null)
                currentRotation = rotations.Last;

            UpdateCamera(currentLocation.Value, currentRotation.Value);
        }
    }

    /// <summary>
    /// Set a new location and rotation for the camera.
    /// </summary>
    /// <param name="location">The new location coordinates.</param>
    /// <param name="rotation">The new rotations angles.</param>
    private void UpdateCamera(Vector3 location, Quaternion rotation)
    {
        transform.position = location;
        transform.rotation = rotation;
    }
}
