// StatBarUI.cs  (class name left unchanged from your current script)
using UnityEngine;
using TMPro;

/// <summary>
/// Displays one numeric stat value.
/// 
/// Caller:
/// The Inspector chooses which stat this instance represents.
/// 

/// - SRP: this class only formats one stat number.
/// - DRY: one shared method handles all stat types.
/// - YAGNI: keeps enum-based logic instead of introducing unnecessary abstractions.
/// </summary>
public class StatNumberUI : MonoBehaviour
{
    public enum StatType
    {
        Energy,
        Hunger,
        Hygiene,
        Stress
    }

    [SerializeField] private GameState state;
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private StatType statType;
    [SerializeField] private string prefix = "";
    [SerializeField] private string suffix = "";

    private void Awake()
    {
        if (state == null) state = FindObjectOfType<GameState>();
    }

    private void OnEnable()
    {
        if (state != null)
        {
            state.OnStatsChanged += UpdateNumber;
        }

        UpdateNumber();
    }

    private void OnDisable()
    {
        if (state != null)
        {
            state.OnStatsChanged -= UpdateNumber;
        }
    }

    private void UpdateNumber()
    {
        if (label == null || state == null) return;

        int value = GetSelectedStatValue();
        label.text = prefix + value + suffix;
    }

    private int GetSelectedStatValue()
    {
        switch (statType)
        {
            case StatType.Energy: return state.GetEnergy();
            case StatType.Hunger: return state.GetHunger();
            case StatType.Hygiene: return state.GetHygiene();
            case StatType.Stress: return state.GetStress();
            default: return 0;
        }
    }
}