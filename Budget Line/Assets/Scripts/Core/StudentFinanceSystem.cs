using UnityEngine;

/// <summary>
/// Handles recurring student finance logic such as:
/// initial loan payment, monthly loan payment, rent countdown, rent payment / missed rent.
/// Day counting is delegated to <see cref="TimeSystem"/> to avoid duplicate state.
/// </summary>
public class StudentFinanceSystem : MonoBehaviour
{
    [Header("Finance")]
    [SerializeField] private int monthlyLoanIncome = 750;
    [SerializeField] public int monthlyRent = 480;

    [Header("Calendar")]
    [SerializeField] private int daysPerMonth = 28;

    [Header("Current Cycle")]
    [SerializeField] private int daysUntilRentDue = 4;

    [Header("References")]
    [SerializeField] private GameState state;
    [SerializeField] private TimeSystem timeSystem;

    private void Awake()
    {
        if (state == null)
            state = FindObjectOfType<GameState>();
        if (timeSystem == null)
            timeSystem = FindObjectOfType<TimeSystem>();

        GiveInitialLoan();
    }

    private void GiveInitialLoan()
    {
        if (state == null) return;

        state.AddMoney(monthlyLoanIncome);
        Debug.Log($"Student loan received: +£{monthlyLoanIncome}");
    }

    /// <summary>
    /// Called once per new day by TimeSystem.
    /// </summary>
    public void OnNewDay()
    {
        UpdateRentCountdown();
        TryGiveMonthlyLoan();
    }

    private void UpdateRentCountdown()
    {
        daysUntilRentDue--;

        if (daysUntilRentDue < 0)
            daysUntilRentDue = daysPerMonth - 1;
    }

    private void TryGiveMonthlyLoan()
    {
        if (state == null || timeSystem == null) return;

        int daysPassed = timeSystem.totalDaysPassed;
        if (daysPassed > 0 && daysPassed % daysPerMonth == 0)
        {
            state.AddMoney(monthlyLoanIncome);
            Debug.Log($"New monthly student loan received: +£{monthlyLoanIncome}");
        }
    }

    public int GetDaysUntilRentDue()
    {
        return daysUntilRentDue;
    }

    public bool IsRentDueToday(TimeSystem ts)
    {
        return ts != null &&
               ts.day == WeekDay.Fri &&
               daysUntilRentDue == 0;
    }

    public bool CanAffordRent()
    {
        return state != null && state.GetMoney() >= monthlyRent;
    }

    public void PayRent()
    {
        if (state == null) return;

        state.AddMoney(-monthlyRent);
        state.rentMissed = false;
        Debug.Log($"Rent paid: -£{monthlyRent}");
    }

    public void MissRent()
    {
        if (state == null) return;

        state.rentMissed = true;
        state.AddStress(+40);
        Debug.Log("Rent not paid. Stress +40");
    }
}
