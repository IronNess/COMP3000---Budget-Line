using UnityEngine;

public class GoalSystem : MonoBehaviour
{
    [Header("Weekly Goal: Study tasks due by Friday")]
    public int tasksRequiredByFriday = 3;
    public int tasksCompletedThisWeek = 0;

    [SerializeField] private GameState state;

    private void Awake()
    {
        if (!state) state = FindObjectOfType<GameState>();
    }

    public void OnStudyTaskCompleted()
    {
        tasksCompletedThisWeek++;
    }

    public void CheckDaily(WeekDay day)
    {
        // Friday deadline check
        if (day == WeekDay.Fri)
        {
            if (tasksCompletedThisWeek < tasksRequiredByFriday)
            {
                // Consequence: stress spike + small grade penalty
                state.AddStress(+15);
                state.AddGrades(-1);
            }

            // Reset for next week (simple model)
            tasksCompletedThisWeek = 0;
        }
    }
}
