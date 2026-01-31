using UnityEngine;
using UnityEngine.UI;

public class DayNightController : MonoBehaviour
{
    public Image overlay;

    [Range(0f, 1f)] public float morningAlpha = 0.00f;
    [Range(0f, 1f)] public float afternoonAlpha = 0.00f;
    [Range(0f, 1f)] public float eveningAlpha = 0.12f;
    [Range(0f, 1f)] public float nightAlpha = 0.25f;

    [Header("Stress Visual Effect")]
    public int stressThreshold = 70;
    [Range(0f, 1f)] public float stressedMinAlpha = 0.40f;

    public float lerpSpeed = 3f;

    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private GameState state;

    private float targetAlpha;

    private void Awake()
    {
        if (!timeSystem) timeSystem = FindObjectOfType<TimeSystem>();
        if (!state) state = FindObjectOfType<GameState>();

        if (timeSystem != null) timeSystem.OnTimeChanged += OnTimeChanged;

        OnTimeChanged();
    }

    private void OnDestroy()
    {
        if (timeSystem != null) timeSystem.OnTimeChanged -= OnTimeChanged;
    }

    private void OnTimeChanged()
    {
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

        // Stress makes the overlay darker (stats affect the world)
        float finalTarget = targetAlpha;
        if (state != null && state.stress > stressThreshold)
        {
            finalTarget = Mathf.Max(finalTarget, stressedMinAlpha);
        }

        Color c = overlay.color;
        c.a = Mathf.Lerp(c.a, finalTarget, Time.deltaTime * lerpSpeed);
        overlay.color = c;
    }
}
