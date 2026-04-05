using UnityEngine;

/// <summary>
/// Tracks weekly academic task goals and their consequences.
/// </summary>
public class GoalSystem : MonoBehaviour
{
    [Header("Weekly Goal: Study tasks due by Friday")]
    [SerializeField] private int tasksRequiredByFriday = 3;
    [SerializeField] public int tasksCompletedThisWeek = 0;

    /// <summary>
    /// True from the Friday evaluation until the following Monday morning tick, if the player was short on study tasks.
    /// </summary>
    public bool MissedDeadlineThisWeek => missedStudyDeadlineCarryover;

    private bool missedStudyDeadlineCarryover;

    [Header("References")]
    [SerializeField] private GameState state;
    [SerializeField] private EventUI eventUI;

    private void Awake()
    {
        if (state == null) state = FindObjectOfType<GameState>();
        if (eventUI == null) eventUI = FindObjectOfType<EventUI>();
    }

    public void OnStudyTaskCompleted()
    {
        tasksCompletedThisWeek++;
    }

    /// <summary>
    /// Called daily by TimeSystem.
    /// </summary>
    public void CheckDaily(WeekDay day)
    {
        if (day == WeekDay.Mon)
            missedStudyDeadlineCarryover = false;

        if (day == WeekDay.Fri)
        {
            EvaluateFridayDeadline();
            ResetWeeklyProgress();
        }
    }

    private void EvaluateFridayDeadline()
    {
        missedStudyDeadlineCarryover = tasksCompletedThisWeek < tasksRequiredByFriday;

        if (missedStudyDeadlineCarryover && state != null)
        {
            state.AddStress(+15);
            state.AddGrades(-1);
        }
    }

    private void ResetWeeklyProgress()
    {
        tasksCompletedThisWeek = 0;
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
        if (missedStudyDeadlineCarryover) message += "You fell behind academically.\n";

        return message;
    }
}
