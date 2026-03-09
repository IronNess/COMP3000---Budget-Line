using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    public GameState state;
    public TextMeshProUGUI label;

    private void Start()
    {
        if (!state)
            state = FindObjectOfType<GameState>();

        if (state != null)
            state.OnStatsChanged += UpdateMoney;

        UpdateMoney();
    }

    private void OnDestroy()
    {
        if (state != null)
            state.OnStatsChanged -= UpdateMoney;
    }

    private void UpdateMoney()
    {
        if (label != null && state != null)
        {
            label.text = "£" + state.GetMoney();
        }
    }
}