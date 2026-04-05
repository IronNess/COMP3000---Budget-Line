using UnityEngine;
using UnityEngine.SceneManagement;

public class CleaningUI : MonoBehaviour
{
    [SerializeField] private string mainSceneName = "MiniGame_Cleaning"; // change this

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Continue()
    {
        SceneManager.LoadScene(mainSceneName);
    }
}