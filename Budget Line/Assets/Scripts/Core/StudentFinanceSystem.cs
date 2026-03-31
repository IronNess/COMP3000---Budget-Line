using UnityEngine;

/// <summary>
/// Handles recurring student finance logic such as:
/// - initial loan payment
/// - monthly loan payment
/// - rent countdown
/// - rent payment / missed rent
/// 
/// Why this is better:
/// - SRP: finance and rent timing are grouped here only.
/// - DRY: shared helper methods avoid repeated loan/rent update logic.
/// - YAGNI: no unnecessary banking/account system is introduced.
/// </summary>
public class StudentFinanceSystem : MonoBehaviour
{
    [Header("Finance")]
    [SerializeField] private int monthlyLoanIncome = 750;
    [SerializeField] public int monthlyRent = 480;

    [Header("Calendar")]
    [SerializeField] private int daysPerMonth = 28;

    [Header("Current Cycle")]
    [SerializeField] private int totalDaysPassed = 0;
    [SerializeField] private int daysUntilRentDue = 4; // Monday start -> Friday due

    [Header("References")]
    [SerializeField] private GameState state;

    private void Awake()
    {
        if (state == null)
        {
            state = FindObjectOfType<GameState>();
        }

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
        totalDaysPassed++;
        UpdateRentCountdown();
        TryGiveMonthlyLoan();
    }

    private void UpdateRentCountdown()
    {
        daysUntilRentDue--;

        if (daysUntilRentDue < 0)
        {
            daysUntilRentDue = daysPerMonth - 1;
        }
    }

    private void TryGiveMonthlyLoan()
    {
        if (state == null) return;

        if (totalDaysPassed > 0 && totalDaysPassed % daysPerMonth == 0)
        {
            state.AddMoney(monthlyLoanIncome);
            Debug.Log($"New monthly student loan received: +£{monthlyLoanIncome}");
        }
    }

    public int GetDaysUntilRentDue()
    {
        return daysUntilRentDue;
    }

    public bool IsRentDueToday(TimeSystem timeSystem)
    {
        return timeSystem != null &&
               timeSystem.day == WeekDay.Fri &&
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
        Debug.Log($"Rent paid: -£{monthlyRent}");
    }

    public void MissRent()
    {
        if (state == null) return;

        state.AddStress(+40);
        Debug.Log("Rent not paid. Stress +40");
    }
}