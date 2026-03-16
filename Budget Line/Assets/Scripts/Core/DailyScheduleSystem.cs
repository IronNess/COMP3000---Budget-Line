using UnityEngine;

public class DailyScheduleSystem : MonoBehaviour
{
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private GameState state;
    [SerializeField] private EventUI eventUI;

    private WeekDay lastDayShown;

    private void Awake()
    {
        if (!timeSystem) timeSystem = FindObjectOfType<TimeSystem>();
        if (!state) state = FindObjectOfType<GameState>();
        if (!eventUI) eventUI = FindObjectOfType<EventUI>();

        lastDayShown = timeSystem.day;
    }

    private void Update()
    {
         if (timeSystem.day != lastDayShown && timeSystem.timeBlock == TimeBlock.Morning)
        {
            lastDayShown = timeSystem.day;
            ShowMorningPrompt();
        }
    }

    private void ShowMorningPrompt()
    { 
        if (timeSystem.day == WeekDay.Mon ||
            timeSystem.day == WeekDay.Tue ||
            timeSystem.day == WeekDay.Wed ||
            timeSystem.day == WeekDay.Thu ||
            timeSystem.day == WeekDay.Fri)
        {
            ShowUniversityPrompt();
            return;
        }
        if (timeSystem.day == WeekDay.Mon)
        {
            ShowRentPrompt();
        }
    }

public void ShowUniversityPrompt()
    {
        EventData temp = ScriptableObject.CreateInstance<EventData>();
        temp.title = "University Today";
        temp.description = "You have university today. Do you want to attend?";

        temp.choices = new EventData.EventChoice[]
        {
            new EventData.EventChoice
            {
                label = "Attend University",
                gradesDelta = +2,
                energyDelta = -8,
                stressDelta = +2,
                timeBlocksCost = 1
            },
            new EventData.EventChoice
            {
                label = "Skip University",
                gradesDelta = -2,
                stressDelta = +3,
                timeBlocksCost = 0
            }
        };

        eventUI.Show(temp);
    }

    public void ShowWorkPrompt()
    {
        EventData temp = ScriptableObject.CreateInstance<EventData>();
        temp.title = "Work Shift Available";
        temp.description = "You have a work shift today. Do you want to go?";

        temp.choices = new EventData.EventChoice[]
        {
            new EventData.EventChoice
            {
                label = "Go to Work",
                moneyDelta = +35,
                energyDelta = -10,
                stressDelta = +3,
                timeBlocksCost = 1
            },
            new EventData.EventChoice
            {
                label = "Decline Shift",
                moneyDelta = 0,
                stressDelta = +2,
                timeBlocksCost = 0
            }
        };

        eventUI.Show(temp);
    }

    public void ShowRentPrompt()
    {
        EventData temp = ScriptableObject.CreateInstance<EventData>();
        temp.title = "Rent Due";
        temp.description = "Your weekly rent is due. You need to pay £120.";

        temp.choices = new EventData.EventChoice[]
        {
            new EventData.EventChoice
            {
                label = "Pay Rent",
                moneyDelta = -120,
                stressDelta = 0
            },
            new EventData.EventChoice
            {
                label = "Miss Rent Payment",
                moneyDelta = 0,
                stressDelta = +10
            }
        };

        eventUI.Show(temp);
    }
}