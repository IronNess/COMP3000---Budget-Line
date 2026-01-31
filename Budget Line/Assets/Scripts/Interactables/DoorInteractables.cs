using UnityEngine;

public class DoorUniversityInteractable : MonoBehaviour, IInteractable
{
    public string Prompt => "Go to University (+Grades, -Energy, +Stress, time passes)";

    [Header("Soft lock settings")]
    public int hygieneThreshold = 20;
    public int lowHygieneStressPenalty = 8;

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
        //  Soft lock: low hygiene makes uni more stressful
        if (state.hygiene < hygieneThreshold)
        {
            state.AddStress(lowHygieneStressPenalty);
        }

        state.AddGrades(+2);
        state.AddEnergy(-20);

        int stressCost = Mathf.RoundToInt(10 * state.resilience);
        state.AddStress(stressCost);

        state.ImproveResilience();

        timeSystem.AdvanceTime(2);

        events.TryTriggerActionEvent();
    }
}
