using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the main menu actions.
/// 
/// Why this is better:
/// - SRP: only main menu behaviour lives here.
/// - YAGNI: no extra scene-loading abstraction is added because it is not needed.
/// </summary>
public class MainMenuController : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string gameplaySceneName = "RoomScene";

    /// <summary>
    /// Called by the Play button.
    /// </summary>
    public void PlayGame()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    /// <summary>
    /// Called by the Exit button.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit pressed");
    }
}