using UnityEngine;
using TMPro;

public class StatNumberUI : MonoBehaviour
{
    public GameState state;
    public TextMeshProUGUI label;

    public enum StatType
    {
        Energy,
        Hunger,
        Hygiene,
        Stress
    }

    public StatType statType;
    public string prefix = "";
    public string suffix = "";

    private void Start()
    {
        if (!state)
            state = FindObjectOfType<GameState>();

        if (state != null)
            state.OnStatsChanged += UpdateNumber;

        UpdateNumber();
    }

    private void OnDestroy()
    {
        if (state != null)
            state.OnStatsChanged -= UpdateNumber;
    }

    private void UpdateNumber()
    {
        if (label == null || state == null) return;

        int value = 0;

        switch (statType)
        {
            case StatType.Energy:
                value = state.GetEnergy();
                break;
            case StatType.Hunger:
                value = state.GetHunger();
                break;
            case StatType.Hygiene:
                value = state.GetHygiene();
                break;
            case StatType.Stress:
                value = state.GetStress();
                break;
        }

        label.text = prefix + value.ToString() + suffix;
    }
}