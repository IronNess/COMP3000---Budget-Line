using UnityEngine;

/// <summary>
/// Shows time-based daily prompts, especially rent reminders and rent-due prompts.
/// 
/// Caller:
/// - Unity lifecycle (Start / Update)
/// - Uses TimeSystem and StudentFinanceSystem state
/// 
/// Why this is better:
/// - SRP: only handles daily reminder UI.
/// - DRY: reminder display logic is split into small focused methods.
/// - YAGNI: no overbuilt scheduling framework is introduced.
/// </summary>
public class DailyScheduleSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private UniversalPopupUI eventUI;
    [SerializeField] private StudentFinanceSystem financeSystem;

    private WeekDay lastDayShown;

    private void Awake()
    {
        ResolveReferences();

        if (timeSystem != null)
        {
            lastDayShown = timeSystem.day;
        }
    }

    private void Start()
    {
        // Show the first reminder immediately if the game begins on Monday morning.
        if (IsCurrentMorning(WeekDay.Mon))
        {
            ShowMorningPrompt();
        }
    }

    private void Update()
    {
        if (timeSystem == null || financeSystem == null) return;

        if (IsNewMorning())
        {
            lastDayShown = timeSystem.day;
            ShowMorningPrompt();
        }
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

    private bool IsNewMorning()
    {
        return timeSystem.day != lastDayShown &&
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
        {
            ShowRentReminder(daysLeft);
        }
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