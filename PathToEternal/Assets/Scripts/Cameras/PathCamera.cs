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
    private List<Vector3> _locationList;

    [SerializeField][Tooltip("All the locations of the camera.")]
    private List<Quaternion> _rotationList;

    [SerializeField][Tooltip("To set a position as perspective or not.")]
    private List<bool> _orthographicList;

    #endregion

    [SerializeField][Tooltip("The duration in second of the camera transitions.")]
    private float _transitionSpeed = 1;

    private LinkedList<Vector3> _locations;
    private LinkedListNode<Vector3> _currentLocation;

    private LinkedList<Quaternion> _rotations;
    private LinkedListNode<Quaternion> _currentRotation;

    private LinkedList<bool> _orthographicPositions;
    private LinkedListNode<bool> _currentOrthographic;

    /// <summary>
    /// Linked lists setup.
    /// </summary>
    private void Awake()
    {
        _locations = new LinkedList<Vector3>();
        _rotations = new LinkedList<Quaternion>();
        _orthographicPositions = new LinkedList<bool>();

        foreach (Vector3 location in _locationList)
            _locations.AddLast(location);

        foreach (Quaternion rotation in _rotationList)
            _rotations.AddLast(rotation);

        foreach (bool perspective in _orthographicList)
            _orthographicPositions.AddLast(perspective);
    }

    /// <summary>
    /// Initialise the current and first location and rotation of the camera.
    /// </summary>
    private void Start()
    {
        _currentLocation = _locations.First;
        _currentRotation = _rotations.First;
        _currentOrthographic = _orthographicPositions.First;

        transform.position = _currentLocation.Value;
        transform.rotation = _currentRotation.Value;
        Camera camera = (Camera)GetComponent("Camera");
        camera.orthographic = _currentOrthographic.Value;
    }

    /// <summary>
    /// Listen to user inputs and prepare for changing of point of view.
    /// </summary>
    private void Update()
    {
        if (LevelGrid.Instance.GameInputsEnabled)
        {
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                _currentLocation = _currentLocation.Next;
                if (_currentLocation == null)
                    _currentLocation = _locations.First;

                _currentRotation = _currentRotation.Next;
                if (_currentRotation == null)
                    _currentRotation = _rotations.First;

                _currentOrthographic = _currentOrthographic.Next;
                if (_currentOrthographic == null)
                    _currentOrthographic = _orthographicPositions.First;
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                _currentLocation = _currentLocation.Previous;
                if (_currentLocation == null)
                    _currentLocation = _locations.Last;

                _currentRotation = _currentRotation.Previous;
                if (_currentRotation == null)
                    _currentRotation = _rotations.Last;

                _currentOrthographic = _currentOrthographic.Previous;
                if (_currentOrthographic == null)
                    _currentOrthographic = _orthographicPositions.Last;
            }
        }
    }

    /// <summary>
    /// Interpolate to new camera location and rotation if it changed.
    /// </summary>
    private void LateUpdate()
    {
        if (transform.position != _currentLocation.Value)
        {
            Camera camera = (Camera)GetComponent("Camera");
            camera.orthographic = _currentOrthographic.Value;

            if (camera.orthographic)
                transform.position = Vector3.Lerp(transform.position, _currentLocation.Value, Time.deltaTime * _transitionSpeed);
            else
                transform.localPosition = Vector3.Lerp(transform.localPosition, _currentLocation.Value, Time.deltaTime * _transitionSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, _currentRotation.Value, Time.deltaTime * _transitionSpeed);
        }
    }
}
