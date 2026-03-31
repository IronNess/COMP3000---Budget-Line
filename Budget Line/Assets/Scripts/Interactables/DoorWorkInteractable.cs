using UnityEngine;

/// <summary>
/// Sends the player to work and applies work-related consequences.
/// 
/// Why this is better:
/// - SRP: only handles work interaction.
/// - DRY: centralised settings replace hard-coded values in Interact().
/// </summary>
public class DoorWorkInteractable : MonoBehaviour, IInteractable
{
    public string Prompt => "Go to Work (+Money, -Energy, -Hunger, -Hygiene, +Stress)";

    [Header("Work Shift Settings")]
    [SerializeField] private int payPerShift = 45;
    [SerializeField] private int timeBlocksCost = 2;
    [SerializeField] private int energyCost = -25;
    [SerializeField] private int hungerCost = -15;
    [SerializeField] private int hygieneCost = -15;
    [SerializeField] private int stressCost = 8;

    [Header("References")]
    [SerializeField] private GameState state;
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private EventManager eventManager;

    private void Awake()
    {
        ResolveReferences();
    }

    private void ResolveReferences()
    {
        if (state == null) state = FindObjectOfType<GameState>();
        if (timeSystem == null) timeSystem = FindObjectOfType<TimeSystem>();
        if (eventManager == null) eventManager = FindObjectOfType<EventManager>();
    }

    public void Interact()
    {
        if (state == null || timeSystem == null) return;

        state.AddMoney(payPerShift);
        state.AddEnergy(energyCost);
        state.AddHunger(hungerCost);
        state.AddHygiene(hygieneCost);
        state.AddStress(stressCost);

        timeSystem.AdvanceTime(timeBlocksCost);
        eventManager?.TryTriggerActionEvent();
    }
}