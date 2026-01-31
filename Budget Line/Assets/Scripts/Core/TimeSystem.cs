using UnityEngine;
using System;

public class TimeSystem : MonoBehaviour
{
    public WeekDay day = WeekDay.Mon;
    public TimeBlock timeBlock = TimeBlock.Morning;

    public event Action OnTimeChanged;

    [SerializeField] private GameState state;
    [SerializeField] private GoalSystem goals;
    [SerializeField] private EventManager events;

    private void Awake()
    {
        if (!state) state = FindObjectOfType<GameState>();
        if (!goals) goals = FindObjectOfType<GoalSystem>();
        if (!events) events = FindObjectOfType<EventManager>();
    }

    public void AdvanceTime(int blocks)
    {
        for (int i = 0; i < blocks; i++)
        {
            timeBlock = NextBlock(timeBlock);

            // wrap to next day
            if (timeBlock == TimeBlock.Morning)
            {
                day = NextDay(day);

                // daily systems tick
                state.ApplyDailyConsequences();
                goals.CheckDaily(day);
                events.TryTriggerDailyEvent();
                FindObjectOfType<StudentFinanceSystem>()?.OnNewDay();
            }
        }

        OnTimeChanged?.Invoke();
    }

    private TimeBlock NextBlock(TimeBlock current)
    {
        return current switch
        {
            TimeBlock.Morning => TimeBlock.Afternoon,
            TimeBlock.Afternoon => TimeBlock.Evening,
            TimeBlock.Evening => TimeBlock.Night,
            _ => TimeBlock.Morning,
        };
    }

    private WeekDay NextDay(WeekDay current)
    {
        return current switch
        {
            WeekDay.Mon => WeekDay.Tue,
            WeekDay.Tue => WeekDay.Wed,
            WeekDay.Wed => WeekDay.Thu,
            WeekDay.Thu => WeekDay.Fri,
            WeekDay.Fri => WeekDay.Sat,
            WeekDay.Sat => WeekDay.Sun,
            _ => WeekDay.Mon,
        };
    }
}
