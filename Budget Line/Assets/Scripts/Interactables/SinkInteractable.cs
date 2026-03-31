using UnityEngine;

/// <summary>
/// Sink interaction:
/// improves hygiene and advances time.
/// 
/// Why this is better:
/// - SRP: only sink logic.
/// - DRY: dependency resolution is centralised.
/// </summary>
public class SinkInteractable : MonoBehaviour, IInteractable
{
    public string Prompt => "Wash (+Hygiene, time passes)";

    [Header("Sink Effects")]
    [SerializeField] private int hygieneGain = 25;
    [SerializeField] private int timeCost = 1;

    [Header("References")]
    [SerializeField] private GameState state;
    [SerializeField] private TimeSystem timeSystem;

    private void Awake()
    {
        ResolveReferences();
    }

    private void ResolveReferences()
    {
        if (state == null) state = FindObjectOfType<GameState>();
        if (timeSystem == null) timeSystem = FindObjectOfType<TimeSystem>();
    }

    public void Interact()
    {
        if (state == null || timeSystem == null) return;

        state.AddHygiene(hygieneGain);
        timeSystem.AdvanceTime(timeCost);
    }
}