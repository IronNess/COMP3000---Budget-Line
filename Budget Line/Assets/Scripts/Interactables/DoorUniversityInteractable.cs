using UnityEngine;

/// <summary>
/// Sends the player to university and applies stat changes.
/// 
/// Why this is better:
/// - SRP: only handles university travel consequences.
/// - DRY: dependency resolution and stress calculation are separated.
/// </summary>
public class DoorUniversityInteractable : MonoBehaviour, IInteractable
{
    public string Prompt => "Go to University (+Grades, -Energy, +Stress, time passes)";

    [Header("Soft Lock Settings")]
    [SerializeField] private int hygieneThreshold = 20;
    [SerializeField] private int lowHygieneStressPenalty = 8;

    [Header("Base Effects")]
    [SerializeField] private int gradeGain = 2;
    [SerializeField] private int energyCost = 20;
    [SerializeField] private int baseStressCost = 10;
    [SerializeField] private int timeCost = 2;

    [Header("References")]
    [SerializeField] private GameState state;
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private EventManager events;

    private void Awake()
    {
        ResolveReferences();
    }

    private void ResolveReferences()
    {
        if (state == null) state = FindObjectOfType<GameState>();
        if (timeSystem == null) timeSystem = FindObjectOfType<TimeSystem>();
        if (events == null) events = FindObjectOfType<EventManager>();
    }

    public void Interact()
    {
        if (state == null || timeSystem == null) return;

        if (state.GetHygiene() < hygieneThreshold)
        {
            state.AddStress(lowHygieneStressPenalty);
        }

        state.AddGrades(gradeGain);
        state.AddEnergy(-energyCost);
        state.AddStress(CalculateStressCost());
        state.ImproveResilience();

        timeSystem.AdvanceTime(timeCost);
        events?.TryTriggerActionEvent();
    }

    private int CalculateStressCost()
    {
        return Mathf.RoundToInt(baseStressCost * state.resilience);
    }
}