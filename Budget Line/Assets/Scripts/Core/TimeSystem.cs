using System;
using UnityEngine;

/// <summary>
/// Handles time progression, day progression, and daily system ticks.
/// 
/// Caller:
/// - Gameplay actions call AdvanceTime(...)
/// 
/// Why this is better:
/// - SRP: central place for time progression.
/// - DRY: daily tick logic is grouped into one method.
/// - YAGNI: enum-based block progression is enough for this project.
/// </summary>
public class TimeSystem : MonoBehaviour
{
    [Header("Current Time")]
    [SerializeField] public WeekDay day = WeekDay.Mon;
    [SerializeField] public TimeBlock timeBlock = TimeBlock.Morning;

    [Header("Progress Tracking")]
    [SerializeField] public int totalDaysPassed = 0;

    public event Action OnTimeChanged;

    /// <summary>
    /// Fired once when the calendar day advances, after the daily tick (stats, goals, events, finance).
    /// </summary>
    public event Action OnNewCalendarDay;

    [Header("References")]
    [SerializeField] private GameState state;
    [SerializeField] private GoalSystem goals;
    [SerializeField] private EventManager events;
    [SerializeField] private StudentFinanceSystem financeSystem;

    private void Awake()
    {
        ResolveReferences();
    }

    private void ResolveReferences()
    {
        if (state == null) state = FindObjectOfType<GameState>();
        if (goals == null) goals = FindObjectOfType<GoalSystem>();
        if (events == null) events = FindObjectOfType<EventManager>();
        if (financeSystem == null) financeSystem = FindObjectOfType<StudentFinanceSystem>();
    }

    /// <summary>
    /// Advances time by the given number of blocks.
    /// This is called by actions such as sleeping, working, studying, etc.
    /// </summary>
    public void AdvanceTime(int blocks)
    {
        if (blocks <= 0) return;
        if (state == null) return;

        for (int i = 0; i < blocks; i++)
        {
            AdvanceSingleBlock();
        }

        OnTimeChanged?.Invoke();
    }

    /// <summary>
    /// Handles the logic for one time block passing.
    /// DRY: keeps repeated progression logic out of AdvanceTime().
    /// </summary>
    private void AdvanceSingleBlock()
    {
        state.ApplyTimeBlockDecay();
        timeBlock = GetNextTimeBlock(timeBlock);

        if (timeBlock == TimeBlock.Morning)
        {
            AdvanceToNextDay();
        }
    }

    private void AdvanceToNextDay()
    {
        day = GetNextDay(day);
        totalDaysPassed++;

        Debug.Log("New day reached. Total days passed: " + totalDaysPassed);

        RunDailySystems();
        OnNewCalendarDay?.Invoke();
    }

    private void RunDailySystems()
    {
        state?.ApplyDailyConsequences();
        goals?.CheckDaily(day);
        events?.TryTriggerDailyEvent();
        financeSystem?.OnNewDay();
    }

    private TimeBlock GetNextTimeBlock(TimeBlock current)
    {
        return current switch
        {
            TimeBlock.Morning => TimeBlock.Afternoon,
            TimeBlock.Afternoon => TimeBlock.Evening,
            TimeBlock.Evening => TimeBlock.Night,
            _ => TimeBlock.Morning,
        };
    }

    private WeekDay GetNextDay(WeekDay current)
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