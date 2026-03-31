using UnityEngine;

/// <summary>
/// Desk interaction:
/// allows the player to study or organise tasks.
/// 
/// Caller:
/// ClickableInteractable -> IInteractable.Interact()
///
/// Why this is better:
/// - SRP: the desk only handles desk-related actions.
/// - DRY: study calculations are grouped into helper methods.
/// - YAGNI: no overengineered task system; still uses simple rules.
/// </summary>
public class DeskInteractable : MonoBehaviour, IInteractable
{
    public string Prompt => "Use Desk";

    [Header("UI")]
    [SerializeField] private InteractionChoiceUI choiceUI;

    [Header("Soft Locks")]
    [SerializeField] private int minEnergyToStudy = 15;
    [SerializeField] private int lowHungerThreshold = 20;
    [SerializeField] private int lowHygieneThreshold = 20;

    [Header("References")]
    [SerializeField] private GameState state;
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private GoalSystem goals;
    [SerializeField] private EventManager events;

    private void Awake()
    {
        ResolveReferences();
    }

    private void ResolveReferences()
    {
        if (choiceUI == null) choiceUI = FindObjectOfType<InteractionChoiceUI>();
        if (state == null) state = FindObjectOfType<GameState>();
        if (timeSystem == null) timeSystem = FindObjectOfType<TimeSystem>();
        if (goals == null) goals = FindObjectOfType<GoalSystem>();
        if (events == null) events = FindObjectOfType<EventManager>();
    }

    public void Interact()
    {
        if (choiceUI == null || state == null || timeSystem == null || goals == null)
            return;

        if (state.GetEnergy() < minEnergyToStudy)
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
        int energyCost = CalculateStudyEnergyCost();
        int stressChange = CalculateStudyStressChange();
        int gradeGain = CalculateStudyGradeGain();
        int timeCost = CalculateStudyTimeCost();

        state.AddGrades(gradeGain);
        state.AddStress(stressChange);
        state.AddEnergy(energyCost);

        goals.OnStudyTaskCompleted();
        state.ImproveStudyEfficiency();

        timeSystem.AdvanceTime(timeCost);
        events?.TryTriggerActionEvent();
    }

    private int CalculateStudyEnergyCost()
    {
        // studyEfficiency reduces the energy loss from studying
        return Mathf.RoundToInt(-15 / state.studyEfficiency);
    }

    private int CalculateStudyStressChange()
    {
        int stressChange = -7;

        if (state.laptopBroken)
            stressChange = 2;

        if (state.GetHygiene() < lowHygieneThreshold)
            stressChange += 2;

        return stressChange;
    }

    private int CalculateStudyGradeGain()
    {
        int gradeGain = 1;

        if (state.laptopBroken)
            gradeGain = 0;

        if (state.GetHunger() < lowHungerThreshold)
            gradeGain -= 1;

        return gradeGain;
    }

    private int CalculateStudyTimeCost()
    {
        return state.laptopBroken ? 2 : 1;
    }

    private void OrganiseTasks()
    {
        state.AddStress(-5);
        timeSystem.AdvanceTime(1);
    }
}