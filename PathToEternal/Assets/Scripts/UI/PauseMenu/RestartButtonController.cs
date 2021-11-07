using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButtonController : MonoBehaviour
{
    /// <summary>
    /// Reload the scene.
    /// </summary>
    public void OnRestartButtonClick() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
