using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    private bool _isActivated;  // To know when to open and close the pause menu

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
        if (Input.GetKeyUp(KeyCode.Escape) && LevelGrid.Instance.GameInputsEnabled)
        {
            if (_isActivated)
                SetPauseMenuActive(false);
            else
                SetPauseMenuActive(true);
        }
    }


    /// <summary>
    /// Activate or deactivate the menu and enable or disable the game inputs.
    /// </summary>
    /// <param name="active">True to activate the menu and disable the game inputs, false to do the opposite.</param>
    private void SetPauseMenuActive(bool active)
    {
        if (active)
            transform.localScale = Vector3.one;
        else
            transform.localScale = Vector3.zero;

        LevelGrid.Instance.GameInputsEnabled = !active;
        _isActivated = active;
    }
}
