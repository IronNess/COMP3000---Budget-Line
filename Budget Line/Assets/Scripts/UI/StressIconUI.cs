using UnityEngine;
using UnityEngine.UI;

public class StressIconUI : MonoBehaviour
{
    [SerializeField] private GameState state;
    [SerializeField] private Image iconImage;

    [Header("Stress Sprites")]
    [SerializeField] private Sprite happySprite;
    [SerializeField] private Sprite stressedSprite;

    [Header("Threshold")]
    [SerializeField] private int stressedThreshold = 50;

    private void Start()
    {
        if (!state)
            state = FindObjectOfType<GameState>();

        if (!iconImage)
            iconImage = GetComponent<Image>();

        if (state != null)
            state.OnStatsChanged += UpdateStressIcon;

        UpdateStressIcon();
    }

    private void OnDestroy()
    {
        if (state != null)
            state.OnStatsChanged -= UpdateStressIcon;
    }

    private void UpdateStressIcon()
    {
        if (state == null || iconImage == null) return;

        int stress = state.GetStress();
        int energy = state.GetEnergy();

        // Show stressed icon if stress is high OR energy is completely empty
        if (stress >= stressedThreshold || energy <= 0)
            iconImage.sprite = stressedSprite;
        else
            iconImage.sprite = happySprite;
    }
}