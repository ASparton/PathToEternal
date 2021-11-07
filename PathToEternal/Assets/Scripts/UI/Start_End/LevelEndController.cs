using UnityEngine;

/// <summary>
/// Controls when to start the level completed animation.
/// </summary>
public class LevelEndController : MonoBehaviour
{
    public static LevelEndController Instance;  // The unique instance of the class to use.

    [SerializeField][Tooltip("The animator containing the level completed animation.")]
    private Animator _animationController;

    /// <summary>
    /// Makes the singleton pattern work.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);
    }

    /// <summary>
    /// Prevents the level completed UI to appear.
    /// </summary>
    private void Start()
    {
        _animationController.enabled = false;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Makes the level completed animation start and disable the in-game inputs.
    /// </summary>
    public void StartAnimation()
    {
        gameObject.SetActive(true);
        _animationController.enabled = true;
        LevelGrid.Instance.GameInputsEnabled = false;
    }
}
