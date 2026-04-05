using UnityEngine;

/// <summary>
/// Shows time-based daily prompts, especially rent reminders and rent-due prompts.
///
/// Caller:
/// - <see cref="TimeSystem.OnNewCalendarDay"/> for morning prompts after each new day
/// - Unity <see cref="Start"/> for the initial Monday-morning prompt when the scene loads
/// </summary>
public class DailyScheduleSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private UniversalPopupUI eventUI;
    [SerializeField] private StudentFinanceSystem financeSystem;

    private void Awake()
    {
        ResolveReferences();
    }

    private void OnEnable()
    {
        if (timeSystem != null)
            timeSystem.OnNewCalendarDay += OnNewCalendarDay;
    }

    private void OnDisable()
    {
        if (timeSystem != null)
            timeSystem.OnNewCalendarDay -= OnNewCalendarDay;
    }

    private void Start()
    {
        if (IsCurrentMorning(WeekDay.Mon))
            ShowMorningPrompt();
    }

    private void OnNewCalendarDay()
    {
        ShowMorningPrompt();
    }

    private void ResolveReferences()
    {
        if (timeSystem == null) timeSystem = FindObjectOfType<TimeSystem>();
        if (eventUI == null) eventUI = FindObjectOfType<UniversalPopupUI>();
        if (financeSystem == null) financeSystem = FindObjectOfType<StudentFinanceSystem>();
    }

    private bool IsCurrentMorning(WeekDay expectedDay)
    {
        return timeSystem != null &&
               timeSystem.day == expectedDay &&
               timeSystem.timeBlock == TimeBlock.Morning;
    }

    private void ShowMorningPrompt()
    {
        if (financeSystem == null || eventUI == null || timeSystem == null) return;

        int daysLeft = financeSystem.GetDaysUntilRentDue();

        if (financeSystem.IsRentDueToday(timeSystem))
        {
            ShowRentPrompt();
            return;
        }

        if (daysLeft == 4 || daysLeft == 2 || daysLeft == 1)
            ShowRentReminder(daysLeft);
    }

    private void ShowRentReminder(int daysLeft)
    {
        if (eventUI == null) return;

        string dayText = daysLeft == 1 ? "day" : "days";

        eventUI.Show(
            "Rent Reminder",
            $"Your monthly rent is due in {daysLeft} {dayText}.",
            null,
            "OK",
            () => { }
        );
    }

    private void ShowRentPrompt()
    {
        if (eventUI == null || financeSystem == null) return;

        eventUI.Show(
            "Rent Due",
            $"Your monthly rent of £{financeSystem.monthlyRent} is due today.",
            null,
            "Pay Rent",
            financeSystem.PayRent,
            "Do Not Pay",
            financeSystem.MissRent
        );
    }
}
