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

    private GenericCamera _currentCamera;   // The current active camera

    private bool _inCinematic;  // To know when the current camera is showing a cinematic (to stop taking game inputs from the player)

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
    }

    /// <summary>
    /// Change the camera view depending on the user inputs.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.C))
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

    /// <summary>
    /// Indicate to the camera controller that a cinematic is played or finished.
    /// </summary>
    /// <param name="inCinematic">True to indicate that a cinematic is playing, false to indicate that it is finished.</param>
    public void SetInCinematic(bool inCinematic) => _inCinematic = inCinematic;

    /// <returns>True if a cinematic is currently playing, false otherwise.</returns>
    public bool IsInCinematic() => _inCinematic;
}
