using UnityEngine;
using UnityEngine.UI;

public class DayNightController : MonoBehaviour
{
    public Image overlay;

    [Range(0f, 1f)] public float morningAlpha = 0.00f;
    [Range(0f, 1f)] public float afternoonAlpha = 0.00f;
    [Range(0f, 1f)] public float eveningAlpha = 0.12f;
    [Range(0f, 1f)] public float nightAlpha = 0.25f;

    public float lerpSpeed = 3f;

    [SerializeField] private TimeSystem timeSystem;

    private float targetAlpha;

    private void Awake()
    {
        if (!timeSystem) timeSystem = FindObjectOfType<TimeSystem>();
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

        Color c = overlay.color;
        c.a = Mathf.Lerp(c.a, targetAlpha, Time.deltaTime * lerpSpeed);
        overlay.color = c;
    }
}
