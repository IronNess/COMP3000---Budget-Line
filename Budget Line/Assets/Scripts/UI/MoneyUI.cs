// MoneyUI.cs
using UnityEngine;
using TMPro;

/// <summary>
/// Updates the money label when the GameState changes.
/// 

/// - SRP: one class, one label.
/// - DRY: single Refresh method reused for start and event updates.
/// </summary>
public class MoneyUI : MonoBehaviour
{
    [SerializeField] private GameState state;
    [SerializeField] private TextMeshProUGUI label;

    private void Awake()
    {
        if (state == null) state = FindObjectOfType<GameState>();
    }

    private void OnEnable()
    {
        if (state != null)
        {
            state.OnStatsChanged += UpdateMoney;
        }

        UpdateMoney();
    }

    private void OnDisable()
    {
        if (state != null)
        {
            state.OnStatsChanged -= UpdateMoney;
        }
    }

    private void UpdateMoney()
    {
        if (label == null || state == null) return;

        label.text = $"£{state.GetMoney()}";
    }
}