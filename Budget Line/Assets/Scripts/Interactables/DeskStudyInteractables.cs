using UnityEngine;

public class DeskStudyInteractable : MonoBehaviour, IInteractable
{
    public string Prompt => "Study (+Grades, -Stress, -Energy, time passes)";

    [SerializeField] private GameState state;
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private GoalSystem goals;
    [SerializeField] private EventManager events;

    private void Awake()
    {
        if (!state) state = FindObjectOfType<GameState>();
        if (!timeSystem) timeSystem = FindObjectOfType<TimeSystem>();
        if (!goals) goals = FindObjectOfType<GoalSystem>();
        if (!events) events = FindObjectOfType<EventManager>();
    }

    public void Interact()
    {
        // progression reduces energy cost over time
        int energyCost = Mathf.RoundToInt(-15 / state.studyEfficiency);

        state.AddGrades(+1);
        state.AddStress(-7);
        state.AddEnergy(energyCost);

        goals.OnStudyTaskCompleted();
        state.ImproveStudyEfficiency();

        timeSystem.AdvanceTime(1);

        // chance of event after studying too
        events.TryTriggerActionEvent();
    }
}
