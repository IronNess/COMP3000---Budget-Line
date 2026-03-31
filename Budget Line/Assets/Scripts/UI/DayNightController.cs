// DayNightController.cs
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the screen overlay used for time-of-day tinting.
/// Also darkens the scene more when stress is high.
/// 
/// 
/// - SRP/SOLID: this class only handles visual overlay behaviour.
/// - DRY: one helper method resolves dependencies.
/// - YAGNI: keeps logic small and focused on current needs only.
/// </summary>
public class DayNightController : MonoBehaviour
{
    [Header("Overlay")]
    [SerializeField] private Image overlay;

    [Header("Time Of Day Alpha")]
    [Range(0f, 1f)] [SerializeField] private float morningAlpha = 0.00f;
    [Range(0f, 1f)] [SerializeField] private float afternoonAlpha = 0.00f;
    [Range(0f, 1f)] [SerializeField] private float eveningAlpha = 0.12f;
    [Range(0f, 1f)] [SerializeField] private float nightAlpha = 0.25f;

    [Header("Stress Visual Effect")]
    [SerializeField] private int stressThreshold = 70;
    [Range(0f, 1f)] [SerializeField] private float stressedMinAlpha = 0.40f;

    [Header("Animation")]
    [SerializeField] private float lerpSpeed = 3f;

    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private GameState state;

    private float targetAlpha;

    private void Awake()
    {
        ResolveReferences();

        if (timeSystem != null)
        {
            timeSystem.OnTimeChanged += OnTimeChanged;
        }

        OnTimeChanged();
    }

    private void OnDestroy()
    {
        if (timeSystem != null)
        {
            timeSystem.OnTimeChanged -= OnTimeChanged;
        }
    }

    private void ResolveReferences()
    {
        if (overlay == null) overlay = GetComponent<Image>();
        if (timeSystem == null) timeSystem = FindObjectOfType<TimeSystem>();
        if (state == null) state = FindObjectOfType<GameState>();
    }

    /// <summary>
    /// Called by TimeSystem when the time block changes.
    /// </summary>
    private void OnTimeChanged()
    {
        if (timeSystem == null) return;

        targetAlpha = timeSystem.timeBlock switch
        {
            TimeBlock.Morning => morningAlpha,
            TimeBlock.Afternoon => afternoonAlpha,
            TimeBlock.Evening => eveningAlpha,
            _ => nightAlpha
        };
    }

    private void Update()
    {
        if (overlay == null) return;

        // Final target is whichever is darker:
        // the time-of-day overlay or the stress overlay.
        float finalTarget = targetAlpha;

        if (state != null && state.GetStress() > stressThreshold)
        {
            finalTarget = Mathf.Max(finalTarget, stressedMinAlpha);
        }

        Color overlayColor = overlay.color;
        overlayColor.a = Mathf.Lerp(overlayColor.a, finalTarget, Time.deltaTime * lerpSpeed);
        overlay.color = overlayColor;
    }
}