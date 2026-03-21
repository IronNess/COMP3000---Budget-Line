using UnityEngine;

public class DoorWorkInteractable : MonoBehaviour
{
    public string Prompt => "Go to Work (+Money, -Energy, -Hunger, -Hygiene, +Stress)";

    [Header("Work Shift Settings")]
    public int payPerShift = 45;       // adjustable
    public int timeBlocksCost = 2;

    public int energyCost = -25;
    public int hungerCost = -15;
    public int hygieneCost = -15;
    public int stressCost = +8;

    [SerializeField] private GameState state;
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private EventManager eventManager;

    private void Awake()
    {
        if (!state) state = FindObjectOfType<GameState>();
        if (!timeSystem) timeSystem = FindObjectOfType<TimeSystem>();
        if (!eventManager) eventManager = FindObjectOfType<EventManager>();
    }

    public void Interact()
    {
        state.AddMoney(payPerShift);
        state.AddEnergy(energyCost);
        state.AddHunger(hungerCost);
        state.AddHygiene(hygieneCost);
        state.AddStress(stressCost);

        timeSystem.AdvanceTime(timeBlocksCost);

        // higher chance of events after work
        eventManager.TryTriggerActionEvent();
    }
}
