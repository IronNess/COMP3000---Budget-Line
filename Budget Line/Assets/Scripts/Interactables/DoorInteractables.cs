using UnityEngine;

public class DoorUniversityInteractable : MonoBehaviour, IInteractable
{
    public string Prompt => "Go to University (+Grades, -Energy, +Stress, time passes)";

    [SerializeField] private GameState state;
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private EventManager events;

    private void Awake()
    {
        if (!state) state = FindObjectOfType<GameState>();
        if (!timeSystem) timeSystem = FindObjectOfType<TimeSystem>();
        if (!events) events = FindObjectOfType<EventManager>();
    }

    public void Interact()
    {
        state.AddGrades(+2);
        state.AddEnergy(-20);

        int stressCost = Mathf.RoundToInt(10 * state.resilience);
        state.AddStress(+stressCost);

        state.ImproveResilience();

        timeSystem.AdvanceTime(2);

        // higher chance of events after leaving home
        events.TryTriggerActionEvent();
    }
}
