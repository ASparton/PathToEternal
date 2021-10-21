using UnityEngine;

/// <summary>
/// Controls the exit portal particle system.
/// </summary>
public class ExitPortalController : Actor
{
    [Header("Exit animation")]
    [SerializeField][Tooltip("Exit animation.")]
    private ParticleSystem EndAnimation;

    [SerializeField][Tooltip("Flame particle system.")]
    private ParticleSystem Flame;

    [SerializeField][Tooltip("The camera used to see end the animation.")]
    private CameraRotator EndAnimationCamera;

    private readonly float _endAnimationDuration = 6.3f;
    // Duration of the end animation
    public float EndAnimationDuration { get { return _endAnimationDuration; } }

    private bool _isFinished = false;

    // Delegate indicating when the exit level animation is finished
    public delegate void ExitAnimationFinished();
    public static event ExitAnimationFinished ExitAnimationFinishedEvent;

    /// <summary>
    /// Notify if the particle systems are all linked or not.
    /// </summary>
    private void Awake()
    {
        if (EndAnimation == null)
            print("WARNING: No end animation found for: " + name);
        if (Flame == null)
            print("WARNING: No flame particle system found for: " + name);
        if (EndAnimationCamera == null)
            print("WARNING: End animation camera not found for: " + name);
    }

    /// <summary>
    /// Start the exit level animation and notify when it's finished.
    /// </summary>
    public void StartExitAnimation()
    {
        if (EndAnimation != null)
            EndAnimation.Play(true);

        if (EndAnimationCamera != null)
            EndAnimationCamera.StartRotate();
    }

    /// <summary>
    /// Used to notify when the exit animation is finished.
    /// </summary>
    private void Update()
    {
        if (!_isFinished && EndAnimation != null && EndAnimation.isPlaying && EndAnimation.time >= _endAnimationDuration) // Animation is finished
        {
            _isFinished = true;

            // Stop all the related animations
            EndAnimation.Stop();
            if (EndAnimationCamera != null)
                EndAnimationCamera.StopRotate();
            if (Flame != null)
                Flame.Stop();

            // Notify the listeners that it is finished
            if (ExitAnimationFinishedEvent != null)
                ExitAnimationFinishedEvent.Invoke();
        }
    }
}
