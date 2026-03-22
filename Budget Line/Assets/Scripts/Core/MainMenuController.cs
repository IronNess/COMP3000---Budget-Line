using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string gameplaySceneName = "RoomScene";

    public void PlayGame()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit pressed");
    }
}