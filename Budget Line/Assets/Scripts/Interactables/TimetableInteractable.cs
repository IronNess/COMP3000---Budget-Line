using UnityEngine;

public class TimetableInteractable : MonoBehaviour, IInteractable
{
    public string Prompt => "Follow Timetable (+Grades, -Energy, time passes)";

    public int gradesGain = 2;
    public int energyCost = -15;
    public int stressChange = -3;
    public int timeBlocksCost = 2;

    [SerializeField] private GameState state;
    [SerializeField] private TimeSystem timeSystem;

    private void Awake()
    {
        if (!state) state = FindObjectOfType<GameState>();
        if (!timeSystem) timeSystem = FindObjectOfType<TimeSystem>();
    }

    public void Interact()
    {
        state.AddGrades(gradesGain);
        state.AddEnergy(energyCost);
        state.AddStress(stressChange);

        timeSystem.AdvanceTime(timeBlocksCost);
    }
}
