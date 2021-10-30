using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;   // The unique instance of the class.

    [SerializeField][Tooltip("The level path camera.")]
    private PathCamera LevelCamera;

    [SerializeField][Tooltip("The player camera.")]
    private ThirdPersonCamera PlayerCamera;

    private GenericCamera _currentCamera;   // The current active camera}

    private bool _inputsEnabled;
    public bool InputsEnabled { get; set; }

    /// <summary>
    /// Assure the singleton pattern.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);
    }

    /// <summary>
    /// Initialise the cameras.
    /// </summary>
    private void Start()
    {
        if (LevelCamera == null)
            print("WARNING: No level path camera provided to the camera controller.");

        if (PlayerCamera == null)
            print("WARNING: No player camera provided to the camera controller.");

        // Enable to level camera first.
        if (PlayerCamera != null && LevelCamera != null)
        {
            _currentCamera = PlayerCamera;
            ActivateCamera(LevelCamera);
        }

        _inputsEnabled = true;
    }

    /// <summary>
    /// Change the camera view depending on the user inputs.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.C) && _inputsEnabled)
        {
            if (LevelCamera.gameObject.activeInHierarchy)
                ActivateCamera(PlayerCamera);
            else  // Means that it's the player camera that is active
                ActivateCamera(LevelCamera);
        }
    }

    /// <summary>
    /// Determine whether the level camera is active or not.
    /// </summary>
    /// <returns>True if the level camera is active, false otherwise.</returns>
    public bool IsLevelCameraActive() => LevelCamera.gameObject.activeInHierarchy;

    /// <summary>
    /// Deactivate the current camera and activate the given one.
    /// </summary>
    public void ActivateCamera(GenericCamera cameraToActivate)
    {
        if (cameraToActivate != _currentCamera)
        {
            _currentCamera.gameObject.SetActive(false);
            cameraToActivate.gameObject.SetActive(true);
            _currentCamera = cameraToActivate;
        }
    }

    /// <returns>The current active camera</returns>
    public GenericCamera GetCurrentCamera() => _currentCamera;
}
