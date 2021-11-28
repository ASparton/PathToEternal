using UnityEngine;

public class ResumeButtonController : MonoBehaviour
{
    [SerializeField][Tooltip("The menu the resume button will activate and deactivate.")]
    private PauseMenuController _pauseMenu;

    /// <summary>
    /// Quit the menu.
    /// </summary>
    public void OnResumeButtonClick() => _pauseMenu.SetPauseMenuActive(false);
}