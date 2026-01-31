using UnityEngine;

public class GoalSystem : MonoBehaviour
{
    [Header("Weekly Goal: Study tasks due by Friday")]
    public int tasksRequiredByFriday = 3;
    public int tasksCompletedThisWeek = 0;

    
    public bool MissedDeadlineThisWeek { get; private set; } = false;

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
       
        if (day == WeekDay.Fri)
        {
          
            MissedDeadlineThisWeek = tasksCompletedThisWeek < tasksRequiredByFriday;

            if (MissedDeadlineThisWeek)
            {
                
                state.AddStress(+15);
                state.AddGrades(-1);
            }

          
            tasksCompletedThisWeek = 0;
            MissedDeadlineThisWeek = false; 
        }
    }

    public void EndOfWeekSummary()
{
    string message = "";

    if (state.energy < 30) message += "You are exhausted.\n";
    if (state.money < 0) message += "You are in debt.\n";
    if (MissedDeadlineThisWeek) message += "You fell behind academically.\n";

    FindObjectOfType<EventUI>().ShowCustom(
        "Weekly Review",
        message
    );
}

}
