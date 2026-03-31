// StressIconUI.cs
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Swaps the stress icon sprite depending on stress/energy.
/// 

/// - SRP: only controls the icon.
/// - DRY: one helper method decides whether the stressed state should show.
/// </summary>
public class StressIconUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameState state;
    [SerializeField] private Image iconImage;

    [Header("Stress Sprites")]
    [SerializeField] private Sprite happySprite;
    [SerializeField] private Sprite stressedSprite;

    [Header("Threshold")]
    [SerializeField] private int stressedThreshold = 50;

    private void Awake()
    {
        if (state == null) state = FindObjectOfType<GameState>();
        if (iconImage == null) iconImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        if (state != null)
        {
            state.OnStatsChanged += UpdateStressIcon;
        }

        UpdateStressIcon();
    }

    private void OnDisable()
    {
        if (state != null)
        {
            state.OnStatsChanged -= UpdateStressIcon;
        }
    }

    private void UpdateStressIcon()
    {
        if (state == null || iconImage == null) return;

        iconImage.sprite = ShouldShowStressedIcon() ? stressedSprite : happySprite;
    }

    private bool ShouldShowStressedIcon()
    {
        return state.GetStress() >= stressedThreshold || state.GetEnergy() <= 0;
    }
}