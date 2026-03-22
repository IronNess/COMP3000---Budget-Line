using UnityEngine;

public class DailyScheduleSystem : MonoBehaviour
{
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private UniversalPopupUI eventUI;
    [SerializeField] private StudentFinanceSystem financeSystem;

    private WeekDay lastDayShown;

    private void Awake()
    {
        if (!timeSystem) timeSystem = FindObjectOfType<TimeSystem>();
        if (!eventUI) eventUI = FindObjectOfType<UniversalPopupUI>();
        if (!financeSystem) financeSystem = FindObjectOfType<StudentFinanceSystem>();

        if (timeSystem != null)
            lastDayShown = timeSystem.day;
    }

    private void Start()
    {
        // Show the first reminder immediately when the game begins
        if (timeSystem != null && timeSystem.day == WeekDay.Mon && timeSystem.timeBlock == TimeBlock.Morning)
        {
            ShowMorningPrompt();
        }
    }

    private void Update()
    {
        if (timeSystem == null || financeSystem == null) return;

        // Detect a new morning/day
        if (timeSystem.day != lastDayShown && timeSystem.timeBlock == TimeBlock.Morning)
{
    lastDayShown = timeSystem.day;
    ShowMorningPrompt();

        }
    }

    private void ShowMorningPrompt()
    {
        if (financeSystem == null || eventUI == null || timeSystem == null) return;

        int daysLeft = financeSystem.GetDaysUntilRentDue();

        // Rent due today
        if (financeSystem.IsRentDueToday(timeSystem))
        {
            ShowRentPrompt();
            return;
        }

        // Reminder popups
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
            () =>
            {
                financeSystem.PayRent();
            },
            "Do Not Pay",
            () =>
            {
                financeSystem.MissRent();
            }
        );
    }
}