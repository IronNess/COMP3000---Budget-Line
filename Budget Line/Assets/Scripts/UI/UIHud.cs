// UIHud.cs
using UnityEngine;
using TMPro;

/// <summary>
/// Updates the main HUD texts when time or stats change.
/// 
/// Why this is better:
/// - SRP: this class only refreshes HUD labels.
/// - DRY: one Refresh method formats all visible values.
/// - YAGNI: keeps formatting local rather than introducing extra format services.
/// </summary>
public class UIHud : MonoBehaviour
{
    [Header("Text Fields")]
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private TMP_Text stressText;
    [SerializeField] private TMP_Text hungerText;
    [SerializeField] private TMP_Text hygieneText;
    [SerializeField] private TMP_Text gradesText;
    [SerializeField] private TMP_Text dayTimeText;

    [Header("Prompt")]
    [SerializeField] private TMP_Text promptText;

    [SerializeField] private GameState state;
    [SerializeField] private TimeSystem timeSystem;

    private void Awake()
    {
        if (state == null) state = FindObjectOfType<GameState>();
        if (timeSystem == null) timeSystem = FindObjectOfType<TimeSystem>();
    }

    private void OnEnable()
    {
        if (state != null)
        {
            state.OnStatsChanged += Refresh;
        }

        if (timeSystem != null)
        {
            timeSystem.OnTimeChanged += Refresh;
        }

        Refresh();
    }

    private void OnDisable()
    {
        if (state != null)
        {
            state.OnStatsChanged -= Refresh;
        }

        if (timeSystem != null)
        {
            timeSystem.OnTimeChanged -= Refresh;
        }
    }

    public void SetPrompt(string msg)
    {
        if (promptText != null)
        {
            promptText.text = msg;
        }
    }

    private void Refresh()
    {
        if (state == null || timeSystem == null) return;

        if (moneyText != null) moneyText.text = $"£{state.GetMoney()}";
        if (energyText != null) energyText.text = $"{state.GetEnergy()}";
        if (stressText != null) stressText.text = $"{state.GetStress()}";
        if (hungerText != null) hungerText.text = $"{state.GetHunger()}";
        if (hygieneText != null) hygieneText.text = $"{state.GetHygiene()}";
        if (gradesText != null) gradesText.text = $"{state.GetGrades()}";
        if (dayTimeText != null) dayTimeText.text = $"{timeSystem.day} - {timeSystem.timeBlock}";
    }
}