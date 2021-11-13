using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverController : MonoBehaviour
{
    public static GameOverController Instance;  // The unique instance of the class to use.

    [SerializeField]
    [Tooltip("The animator containing the game over animation.")]
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
    /// Prevents the game over UI to appear.
    /// </summary>
    private void Start()
    {
        _animationController.enabled = false;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Makes the game over animation start.
    /// </summary>
    public void StartAnimation()
    {
        gameObject.SetActive(true);
        _animationController.enabled = true;
    }
}
