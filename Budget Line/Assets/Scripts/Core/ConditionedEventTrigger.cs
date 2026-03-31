using UnityEngine;

/// <summary>
/// Triggers context-sensitive events based on player state and recent progress.
/// 
/// Examples:
/// - low hygiene can lead to dentist events
/// - heavy study load can lead to broken laptop events
/// - low money can increase equipment/fine-related events
/// 
/// Why this is better:
/// - SRP: only handles condition-based event triggering.
/// - DRY: event choice and chance logic are split into helpers.
/// - YAGNI: still uses simple threshold rules rather than a large event-rule system.
/// </summary>
public class ConditionedEventTrigger : MonoBehaviour
{
    [Header("Event Assets")]
    [SerializeField] private EventData brokenLaptop;
    [SerializeField] private EventData dentistCost;
    [SerializeField] private EventData libraryFine;
    [SerializeField] private EventData academicEquipment;

    [Header("Trigger Timing")]
    [SerializeField] private int minDaysBetweenEvents = 2;
    private int daysSinceLast = 999;

    [Header("Condition Thresholds")]
    [SerializeField] private int lowHygieneThreshold = 25;
    [SerializeField] private int highStudyThreshold = 4;
    [SerializeField] private int lowMoneyThreshold = 20;

    [Header("Base Chances")]
    [Range(0f, 1f)] [SerializeField] private float baseChancePerDay = 0.05f;
    [Range(0f, 1f)] [SerializeField] private float hygieneBonusChance = 0.12f;
    [Range(0f, 1f)] [SerializeField] private float studyBonusChance = 0.10f;
    [Range(0f, 1f)] [SerializeField] private float lowMoneyBonusChance = 0.08f;
    [Range(0f, 1f)] [SerializeField] private float missedDeadlineBonusChance = 0.10f;

    [Header("References")]
    [SerializeField] private GameState state;
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private GoalSystem goals;
    [SerializeField] private EventUI eventUI;

    private WeekDay lastDay;

    private void Awake()
    {
        ResolveReferences();

        if (timeSystem != null)
        {
            lastDay = timeSystem.day;
        }
    }

    private void Update()
    {
        if (timeSystem == null) return;

        if (timeSystem.day != lastDay)
        {
            lastDay = timeSystem.day;
            daysSinceLast++;
            TryTrigger();
        }
    }

    private void ResolveReferences()
    {
        if (state == null) state = FindObjectOfType<GameState>();
        if (timeSystem == null) timeSystem = FindObjectOfType<TimeSystem>();
        if (goals == null) goals = FindObjectOfType<GoalSystem>();
        if (eventUI == null) eventUI = FindObjectOfType<EventUI>();
    }

    private void TryTrigger()
    {
        if (!CanTriggerToday()) return;

        float chance = CalculateTriggerChance();
        if (Random.value > chance) return;

        EventData chosenEvent = ChooseEvent();
        if (chosenEvent == null || eventUI == null) return;

        daysSinceLast = 0;
        eventUI.Show(chosenEvent);
    }

    private bool CanTriggerToday()
    {
        return daysSinceLast >= minDaysBetweenEvents &&
               state != null &&
               goals != null;
    }

    private float CalculateTriggerChance()
    {
        float chance = baseChancePerDay;

        if (IsLowHygiene()) chance += hygieneBonusChance;
        if (IsHeavyStudyLoad()) chance += studyBonusChance;
        if (IsLowMoney()) chance += lowMoneyBonusChance;
        if (goals.MissedDeadlineThisWeek) chance += missedDeadlineBonusChance;

        return chance;
    }

    private EventData ChooseEvent()
    {
        if (IsLowHygiene() && dentistCost != null)
            return dentistCost;

        if (IsHeavyStudyLoad() && brokenLaptop != null)
            return brokenLaptop;

        if (goals.MissedDeadlineThisWeek && libraryFine != null)
            return libraryFine;

        return ChooseFallbackEvent();
    }

    private EventData ChooseFallbackEvent()
    {
        float randomValue = Random.value;

        if (IsLowMoney() && academicEquipment != null)
            return academicEquipment;

        if (randomValue < 0.5f && academicEquipment != null)
            return academicEquipment;

        return libraryFine;
    }

    private bool IsLowHygiene()
    {
        return state.GetHygiene() <= lowHygieneThreshold;
    }

    private bool IsHeavyStudyLoad()
    {
        return goals.tasksCompletedThisWeek >= highStudyThreshold;
    }

    private bool IsLowMoney()
    {
        return state.GetMoney() <= lowMoneyThreshold;
    }
}