using UnityEngine;

public class ConditionedEventTrigger : MonoBehaviour
{
    [Header("Event Assets (EventData .asset)")]
    public EventData brokenLaptop;
    public EventData dentistCost;
    public EventData libraryFine;
    public EventData academicEquipment;

    [Header("How often can a conditioned event happen?")]
    public int minDaysBetweenEvents = 2;
    private int daysSinceLast = 999;

    [Header("Condition thresholds")]
    public int lowHygieneThreshold = 25;      // dentist more likely below this
    public int highStudyThreshold = 4;        // laptop more likely above this
    public int lowMoneyThreshold = 20;        // equipment/fine more likely below this

    [Header("Base chances")]
    [Range(0f, 1f)] public float baseChancePerDay = 0.05f;
    [Range(0f, 1f)] public float hygieneBonusChance = 0.12f;
    [Range(0f, 1f)] public float studyBonusChance = 0.10f;
    [Range(0f, 1f)] public float lowMoneyBonusChance = 0.08f;

    [SerializeField] private GameState state;
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private GoalSystem goals;
    [SerializeField] private EventUI eventUI;

    private WeekDay lastDay;

    private void Awake()
    {
        if (!state) state = FindObjectOfType<GameState>();
        if (!timeSystem) timeSystem = FindObjectOfType<TimeSystem>();
        if (!goals) goals = FindObjectOfType<GoalSystem>();
        if (!eventUI) eventUI = FindObjectOfType<EventUI>();

        lastDay = timeSystem.day;
    }

    private void Update()
    {
        // detect day change
        if (timeSystem.day != lastDay)
        {
            lastDay = timeSystem.day;
            daysSinceLast++;
            TryTrigger();
        }
    }

    private void TryTrigger()
    {
        if (daysSinceLast < minDaysBetweenEvents) return;

        float chance = baseChancePerDay;

        bool lowHygiene = state.hygiene <= lowHygieneThreshold;
        bool highStudy = goals.tasksCompletedThisWeek >= highStudyThreshold;
        bool lowMoney = state.money <= lowMoneyThreshold;
        bool missedDeadline = goals.MissedDeadlineThisWeek; 

        if (lowHygiene) chance += hygieneBonusChance;
        if (highStudy) chance += studyBonusChance;
        if (lowMoney) chance += lowMoneyBonusChance;

        // missing deadlines increases chance of fine
        if (missedDeadline) chance += 0.10f;

        if (Random.value > chance) return;

        // pick the best matching event
        EventData chosen = null;

        if (lowHygiene && dentistCost != null)
            chosen = dentistCost;
        else if (highStudy && brokenLaptop != null)
            chosen = brokenLaptop;
        else if (missedDeadline && libraryFine != null)
            chosen = libraryFine;
        else
        {
            // fallback: equipment if low money or just random
            float r = Random.value;
            if (lowMoney && academicEquipment != null) chosen = academicEquipment;
            else if (r < 0.5f && academicEquipment != null) chosen = academicEquipment;
            else if (libraryFine != null) chosen = libraryFine;
        }

        if (chosen != null)
        {
            daysSinceLast = 0;
            eventUI.Show(chosen);
        }
    }
}
