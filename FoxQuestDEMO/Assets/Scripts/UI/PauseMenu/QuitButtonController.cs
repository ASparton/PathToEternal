using UnityEngine;

public class QuitButtonController : MonoBehaviour
{
    /// <summary>
    /// Quit the game.
    /// </summary>
    public void OnQuitButtonClick() => Application.Quit();
}