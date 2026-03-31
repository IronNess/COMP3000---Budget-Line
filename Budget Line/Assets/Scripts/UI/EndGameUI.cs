// EndGameUI.cs
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Displays the final end-of-game screen.
/// 
/// Caller:
/// EndingSystem calls ShowEndScreen(...) when the player wins or fails.
/// 
/// - SRP/SOLID: this class only handles end-screen presentation.
/// - DRY: grade conversion uses GradeUtility; formatting is split into helpers.
/// - YAGNI: no extra systems or abstractions that the project does not need.
/// </summary>
public class EndGameUI : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject panel;

    [Header("Main Text")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text summaryText;
    [SerializeField] private TMP_Text survivalPercentText;

    [Header("Stat Text")]
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private TMP_Text hungerText;
    [SerializeField] private TMP_Text hygieneText;
    [SerializeField] private TMP_Text stressText;
    [SerializeField] private TMP_Text gradesText;

    [Header("Scene References")]
    [SerializeField] private GameState state;
    [SerializeField] private UniversalPopupUI popupUI;
    [SerializeField] private GameObject gameplayUIRoot;

    private bool hasShown;

    private void Awake()
    {
        ResolveReferences();

        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    private void ResolveReferences()
    {
        if (state == null) state = FindObjectOfType<GameState>();
        if (popupUI == null) popupUI = FindObjectOfType<UniversalPopupUI>();
    }

    /// <summary>
    /// Called by EndingSystem.
    /// Shows the panel once and freezes the game.
    /// </summary>
    public void ShowEndScreen(string title = "Game Over")
    {
        if (hasShown) return;

        hasShown = true;
        Debug.Log("ShowEndScreen called with title: " + title);

        if (state == null || panel == null) return;

        HideGameplayUI();
        PopulateText(title);

        panel.SetActive(true);
        panel.transform.SetAsLastSibling();

        // Time.timeScale pauses time-based gameplay and animations.
        Time.timeScale = 0f;
    }

    private void HideGameplayUI()
    {
        if (popupUI != null)
        {
            popupUI.Hide();
        }

        if (gameplayUIRoot != null)
        {
            gameplayUIRoot.SetActive(false);
        }
    }

    private void PopulateText(string title)
    {
        int survivalPercent = CalculateSurvivalPercent();
        string summary = BuildSummary(survivalPercent);
        string gradeLetter = GradeUtility.ToLetter(state.GetGrades());

        if (titleText != null) titleText.text = title;
        if (summaryText != null) summaryText.text = summary;
        if (survivalPercentText != null) survivalPercentText.text = $"Overall Survival: {survivalPercent}%";

        if (moneyText != null) moneyText.text = $"Money: £{state.GetMoney()}";
        if (energyText != null) energyText.text = $"Energy: {state.GetEnergy()}";
        if (hungerText != null) hungerText.text = $"Hunger: {state.GetHunger()}";
        if (hygieneText != null) hygieneText.text = $"Hygiene: {state.GetHygiene()}";
        if (stressText != null) stressText.text = $"Stress: {state.GetStress()}";
        if (gradesText != null) gradesText.text = $"Grades: {gradeLetter}";
    }

    private int CalculateSurvivalPercent()
    {
        int energyScore = state.GetEnergy();
        int hungerScore = state.GetHunger();
        int hygieneScore = state.GetHygiene();
        int stressScore = 100 - state.GetStress();
        int gradesScore = Mathf.Clamp(state.GetGrades(), 0, 100);
        int moneyScore = CalculateMoneyScore(state.GetMoney());

        return Mathf.RoundToInt(
            (energyScore + hungerScore + hygieneScore + stressScore + gradesScore + moneyScore) / 6f
        );
    }

    private int CalculateMoneyScore(int money)
    {
        if (money < 0) return 0;
        if (money >= 200) return 100;
        return Mathf.RoundToInt((money / 200f) * 100f);
    }

    private string BuildSummary(int survivalPercent)
    {
        if (survivalPercent >= 80)
            return "You managed student life very well and balanced your responsibilities.";

        if (survivalPercent >= 60)
            return "You survived, but several areas of your life became difficult to maintain.";

        if (survivalPercent >= 40)
            return "You struggled to keep up with daily life and the pressure clearly built up.";

        return "Life became overwhelming, and survival was extremely difficult.";
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