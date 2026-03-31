using UnityEngine;

/// <summary>
/// Tracks weekly academic task goals and their consequences.
/// 
/// Why this is better:
/// - SRP: only academic goal tracking lives here.
/// - DRY: Friday evaluation is grouped into a single helper flow.
/// - YAGNI: keeps a simple weekly goal model without building a full planner system.
/// </summary>
public class GoalSystem : MonoBehaviour
{
    [Header("Weekly Goal: Study tasks due by Friday")]
    [SerializeField] private int tasksRequiredByFriday = 3;
    [SerializeField] public int tasksCompletedThisWeek = 0;

    public bool MissedDeadlineThisWeek { get; private set; } = false;

    [Header("References")]
    [SerializeField] private GameState state;
    [SerializeField] private EventUI eventUI;

    private void Awake()
    {
        if (state == null) state = FindObjectOfType<GameState>();
        if (eventUI == null) eventUI = FindObjectOfType<EventUI>();
    }

    /// <summary>
    /// Called when a study task is completed.
    /// </summary>
    public void OnStudyTaskCompleted()
    {
        tasksCompletedThisWeek++;
    }

    /// <summary>
    /// Called daily by TimeSystem.
    /// </summary>
    public void CheckDaily(WeekDay day)
    {
        if (day == WeekDay.Fri)
        {
            EvaluateFridayDeadline();
            ResetWeeklyProgress();
        }
    }

    private void EvaluateFridayDeadline()
    {
        MissedDeadlineThisWeek = tasksCompletedThisWeek < tasksRequiredByFriday;

        if (MissedDeadlineThisWeek && state != null)
        {
            state.AddStress(+15);
            state.AddGrades(-1);
        }
    }

    private void ResetWeeklyProgress()
    {
        tasksCompletedThisWeek = 0;

        // Kept to match your current project behaviour.
        // This means the "missed deadline" state is temporary after Friday processing.
        MissedDeadlineThisWeek = false;
    }

    public void EndOfWeekSummary()
    {
        if (eventUI == null || state == null) return;

        string message = BuildWeeklySummaryMessage();

        eventUI.ShowCustom(
            "Weekly Review",
            message
        );
    }

    private string BuildWeeklySummaryMessage()
    {
        string message = "";

        if (state.GetEnergy() < 30) message += "You are exhausted.\n";
        if (state.GetMoney() < 0) message += "You are in debt.\n";
        if (MissedDeadlineThisWeek) message += "You fell behind academically.\n";

        return message;
    }
}