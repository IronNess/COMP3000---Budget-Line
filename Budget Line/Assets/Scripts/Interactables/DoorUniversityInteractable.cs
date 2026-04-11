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
    public string Prompt => "Go to University (+Grades, other needs go down, +Stress, time passes)";

    [Header("Soft Lock Settings")]
    [SerializeField] private int hygieneThreshold = 20;
    [SerializeField] private int lowHygieneStressPenalty = 8;

    [Header("Base Effects")]
    [SerializeField] private int gradeGain = 3;
    [SerializeField] private int energyCost = 20;
    [SerializeField] private int baseStressCost = 10;
    [SerializeField] private int timeCost = 2;

    [Header("Other needs (day at university)")]
    [SerializeField] private int hungerCost = 10;
    [SerializeField] private int hygieneCost = 6;
    [Tooltip("Optional bus / travel spend. Set 0 to disable.")]
    [SerializeField] private int travelMoneyCost = 0;

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

    /// <summary>
    /// Called from <see cref="TravelMenuUI"/> when the scene may not contain a door object with this component.
    /// Finds any instance (including inactive), or applies defaults on a temporary object so stats still change.
    /// </summary>
    public static void PlayUniversityTripFromMenu()
    {
        DoorUniversityInteractable door = FindFirstObjectByType<DoorUniversityInteractable>(FindObjectsInactive.Include);
        if (door != null)
        {
            door.Interact();
            return;
        }

        GameObject temp = new GameObject("_RuntimeUniversityTrip");
        DoorUniversityInteractable runtime = temp.AddComponent<DoorUniversityInteractable>();
        runtime.Interact();
        Object.Destroy(temp);
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
        state.AddHunger(-hungerCost);
        state.AddHygiene(-hygieneCost);

        if (travelMoneyCost > 0)
            state.AddMoney(-travelMoneyCost);

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