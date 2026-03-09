using UnityEngine;
using UnityEngine.UI;

public class StatBarUI : MonoBehaviour
{
    public GameState state;
    public Image fillImage;

    public enum StatType
    {
        Energy,
        Hunger,
        Hygiene,
        Stress
    }

    public StatType statType;

    void Start()
    {
        if (!state)
            state = FindObjectOfType<GameState>();

        state.OnStatsChanged += UpdateBar;

        UpdateBar();
    }

    void UpdateBar()
    {
        float value = 0f;

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

        fillImage.fillAmount = value / 100f;
    }
}