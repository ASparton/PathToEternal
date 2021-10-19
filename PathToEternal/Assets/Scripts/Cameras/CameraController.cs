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

    [SerializeField][Tooltip("The end animation camera.")]
    private CameraRotator EndAnimationCamera;

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

        if (EndAnimationCamera == null)
            print("WARNING: No end animation camera provided to the camera controller.");

        // Enable to level camera first.
        if (PlayerCamera != null && LevelCamera != null)
        {
            ActivateLevelCamera();
            EndAnimationCamera.gameObject.SetActive(false);
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
                ActivatePlayerCamera();
            else  // Means that it's the player camera that is active
                ActivateLevelCamera();
        }
    }

    /// <summary>
    /// Determine whether the level camera is active or not.
    /// </summary>
    /// <returns>True if the level camera is active, false otherwise.</returns>
    public bool IsLevelCameraActive() => LevelCamera.gameObject.activeInHierarchy;

    /// <summary>
    /// Desactivate the player and level camera and activate the end animation camera.
    /// </summary>
    public void ActivateEndAnimationCamera()
    {
        EndAnimationCamera.gameObject.SetActive(true);
        PlayerCamera.gameObject.SetActive(false);
        LevelCamera.gameObject.SetActive(false);
    }

    /// <summary>
    /// Desactivate the player camera and activate the level camera.
    /// </summary>
    private void ActivateLevelCamera()
    {
        LevelCamera.gameObject.SetActive(true);
        PlayerCamera.gameObject.SetActive(false);
    }

    /// <summary>
    /// Desactivate the level camera and activate the player camera.
    /// </summary>
    private void ActivatePlayerCamera()
    {
        LevelCamera.gameObject.SetActive(false);
        PlayerCamera.gameObject.SetActive(true);
    }
}
