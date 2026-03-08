using UnityEngine;

public class DeskInteractable : MonoBehaviour, IInteractable
{
    public string Prompt => "Use Desk";

    [SerializeField] private InteractionChoiceUI choiceUI;

    [Header("Soft locks")]
    public int minEnergyToStudy = 15;
    public int lowHungerThreshold = 20;
    public int lowHygieneThreshold = 20;

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
        if (!choiceUI)
        {
            Debug.LogError("DeskInteractable: choiceUI not assigned in Inspector.");
            return;
        }

        // Soft lock: too tired to study
        if (state.energy < minEnergyToStudy)
        {
            choiceUI.Show("Too Tired", "You are too exhausted to focus right now.");
            return;
        }

        choiceUI.Show(
            "Desk",
            "What do you want to do?",
            new InteractionChoice("Study", Study),
            new InteractionChoice("Organise Tasks", OrganiseTasks)
        );
    }

    private void Study()
    {
        // Base study costs
        int energyCost = Mathf.RoundToInt(-15 / state.studyEfficiency);
        int stressChange = -7;
        int timeCost = 1;
        int gradeGain = 1;

        // Ongoing problem: broken laptop makes studying slower + less effective
        if (state.laptopBroken)
        {
            timeCost = 2;
            gradeGain = 0; // or keep 1 but make it harder — up to you
            stressChange = +2;
        }

        // Soft lock penalties (planning pressure)
        if (state.hunger < lowHungerThreshold) gradeGain -= 1;     // worse grades when hungry
        if (state.hygiene < lowHygieneThreshold) stressChange += 2; // distracted/embarrassed

        // Apply
        state.AddGrades(gradeGain);
        state.AddStress(stressChange);
        state.AddEnergy(energyCost);

        goals.OnStudyTaskCompleted();
        state.ImproveStudyEfficiency();

        timeSystem.AdvanceTime(timeCost);
        events.TryTriggerActionEvent();
    }

    private void OrganiseTasks()
    {
        // Organising reduces stress but still takes time
        state.AddStress(-5);
        timeSystem.AdvanceTime(1);
    }
}
