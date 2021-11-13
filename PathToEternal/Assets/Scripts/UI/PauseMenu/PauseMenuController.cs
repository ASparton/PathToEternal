using System.Collections;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField][Tooltip("The duration of the display animation.")]
    private float _displayAnimationDuration = 0.3f;

    private bool _isActivated;  // To know when to open and close the pause menu
    private bool _inAnimation;  // To disable the escape input during a hide or display animation

    private void Start()
    {
        transform.localScale = Vector3.zero;
        _isActivated = false;
    }

    /// <summary>
    /// Activate or deactivate the pause menu.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && !_inAnimation)
        {
            if (_isActivated)
                SetPauseMenuActive(false);
            else
            {
                if (LevelGrid.Instance.GameInputsEnabled)
                    SetPauseMenuActive(true);
            }
        }
    }


    /// <summary>
    /// Activate or deactivate the menu and enable or disable the game inputs.
    /// </summary>
    /// <param name="active">True to activate the menu and disable the game inputs, false to do the opposite.</param>
    public void SetPauseMenuActive(bool active)
    {
        if (active)
            StartCoroutine(DisplayMenu());
        else
            StartCoroutine(HideMenu());

        LevelGrid.Instance.GameInputsEnabled = !active;
        _isActivated = active;
    }

    #region Animations methods

    /// <summary>
    /// Lerp from the menu position from its hide position to its display position.
    /// </summary>
    /// <returns>Coroutine</returns>
    private IEnumerator DisplayMenu()
    {
        _inAnimation = true;

        float timeElapsed = 0f;
        while (timeElapsed < _displayAnimationDuration)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, timeElapsed / _displayAnimationDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = Vector3.one;
        _inAnimation = false;
    }

    /// <summary>
    /// Lerp from the menu position from its display position to its hide position.
    /// </summary>
    /// <returns>Coroutine</returns>
    private IEnumerator HideMenu()
    {
        _inAnimation = true;

        float timeElapsed = 0f;
        while (timeElapsed < _displayAnimationDuration)
        {
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timeElapsed / _displayAnimationDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = Vector3.zero;
        _inAnimation = false;
    }

    #endregion
}
