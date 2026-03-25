using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    private void Awake()
    {
        if (panel == null)
            panel = gameObject;

        panel.SetActive(false);
    }

    public void Open()
    {
        panel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Close()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OpenCleaningGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MiniGame_Cleaning");
    }

    public void OpenMissingWordsGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MiniGame_MissingWords");
    }

    public void OpenSandwichGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MiniGame_Sandwich");
    }

    public void OpenEnergyBarGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MiniGame_EnergyBar");
    }
}