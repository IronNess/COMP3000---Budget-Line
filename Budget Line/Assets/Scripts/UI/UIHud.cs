using UnityEngine;
using TMPro;

public class UIHud : MonoBehaviour
{
    [Header("Text Fields")]
    public TMP_Text moneyText;
    public TMP_Text energyText;
    public TMP_Text stressText;
    public TMP_Text hungerText;
    public TMP_Text hygieneText;
    public TMP_Text gradesText;
    public TMP_Text dayTimeText;

    [Header("Prompt")]
    public TMP_Text promptText;

    [SerializeField] private GameState state;
    [SerializeField] private TimeSystem timeSystem;

    private void Awake()
    {
        if (!state) state = FindObjectOfType<GameState>();
        if (!timeSystem) timeSystem = FindObjectOfType<TimeSystem>();

        state.OnStatsChanged += Refresh;
        timeSystem.OnTimeChanged += Refresh;

        Refresh();
    }

    public void SetPrompt(string msg)
    {
        if (promptText) promptText.text = msg;
    }

private void Refresh()
{
    if (moneyText) moneyText.text = $"£{state.money}";
    if (energyText) energyText.text = $"{state.energy}";
    if (stressText) stressText.text = $"{state.stress}";
    if (hungerText) hungerText.text = $"{state.hunger}";
    if (hygieneText) hygieneText.text = $"{state.hygiene}";
    if (gradesText) gradesText.text = $"{state.grades}";

    if (dayTimeText) dayTimeText.text = $"{timeSystem.day} - {timeSystem.timeBlock}";
}
}
