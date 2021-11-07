using UnityEngine;

public class ResumeButtonController : MonoBehaviour
{
    [SerializeField][Tooltip("The menu the resume button will activate and deactivate.")]
    private RectTransform _pauseMenu;

    /// <summary>
    /// Quit the menu.
    /// </summary>
    public void OnResumeButtonClick()
    {
        _pauseMenu.gameObject.transform.localScale = Vector3.zero;
        LevelGrid.Instance.GameInputsEnabled = true;
    }
}