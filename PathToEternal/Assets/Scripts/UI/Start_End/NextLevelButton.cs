using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelButton : MonoBehaviour
{
    [SerializeField][Tooltip("The name of the next level scene to load.")]
    private string _nextLevelSceneName;

    /// <summary>
    /// Load the next level scene.
    /// </summary>
    public void OnNextLevelButtonClick() => print("next level"); //=> SceneManager.LoadScene(_nextLevelSceneName);
}