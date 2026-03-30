using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndGameUI : MonoBehaviour
{
    [Header("Panel")]
    public GameObject panel;

    [Header("Main Text")]
    public TMP_Text titleText;
    public TMP_Text summaryText;
    public TMP_Text survivalPercentText;

    [Header("Stat Text")]
    public TMP_Text moneyText;
    public TMP_Text energyText;
    public TMP_Text hungerText;
    public TMP_Text hygieneText;
    public TMP_Text stressText;
    public TMP_Text gradesText;

    

    [SerializeField] private GameState state;
    [SerializeField] private UniversalPopupUI popupUI;
    [SerializeField] private GameObject gameplayUIRoot;

    private void Awake()
    {
        if (!state)
            state = FindObjectOfType<GameState>();

        if (!popupUI)
            popupUI = FindObjectOfType<UniversalPopupUI>();

        if (panel != null)
            panel.SetActive(false);
    }

    public void ShowEndScreen(string title = "Game Over")
    {
        Debug.Log("ShowEndScreen called with title: " + title);
        if (state == null || panel == null) return;

        int energyScore = state.GetEnergy();
        int hungerScore = state.GetHunger();
        int hygieneScore = state.GetHygiene();
        int stressScore = 100 - state.GetStress();
        int gradesScore = Mathf.Clamp(state.GetGrades(), 0, 100);

        int moneyScore;
        if (state.GetMoney() < 0)
            moneyScore = 0;
        else if (state.GetMoney() >= 200)
            moneyScore = 100;
        else
            moneyScore = Mathf.RoundToInt((state.GetMoney() / 200f) * 100f);

        int survivalPercent = Mathf.RoundToInt(
            (energyScore + hungerScore + hygieneScore + stressScore + gradesScore + moneyScore) / 6f
        );

        string gradeLetter = "D";
        if (state.GetGrades() >= 80) gradeLetter = "A";
        else if (state.GetGrades() >= 60) gradeLetter = "B";
        else if (state.GetGrades() >= 40) gradeLetter = "C";

        string summary;
        if (survivalPercent >= 80)
            summary = "You managed student life very well and balanced your responsibilities.";
        else if (survivalPercent >= 60)
            summary = "You survived, but several areas of your life became difficult to maintain.";
        else if (survivalPercent >= 40)
            summary = "You struggled to keep up with daily life and the pressure clearly built up.";
        else
            summary = "Life became overwhelming, and survival was extremely difficult.";

        if (popupUI != null)
    popupUI.Hide();

if (gameplayUIRoot != null)
    gameplayUIRoot.SetActive(false);

panel.SetActive(true);
panel.transform.SetAsLastSibling();

        titleText.text = title;
        summaryText.text = summary;
        survivalPercentText.text = "Overall Survival: " + survivalPercent + "%";

        moneyText.text = "Money: £" + state.GetMoney();
        energyText.text = "Energy: " + state.GetEnergy();
        hungerText.text = "Hunger: " + state.GetHunger();
        hygieneText.text = "Hygiene: " + state.GetHygiene();
        stressText.text = "Stress: " + state.GetStress();
        gradesText.text = "Grades: " + gradeLetter;

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}