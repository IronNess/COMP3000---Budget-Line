using UnityEngine;

public class StudentFinanceSystem : MonoBehaviour
{
    [Header("Finance")]
    public int monthlyLoanIncome = 750;
    public int monthlyRent = 480;

    [Header("Calendar")]
    public int daysPerMonth = 28;

    [Header("Current Cycle")]
    [SerializeField] private int totalDaysPassed = 0;
    [SerializeField] private int daysUntilRentDue = 4; // Monday start -> Friday due

    [SerializeField] private GameState state;

    private void Awake()
    {
        if (!state) state = FindObjectOfType<GameState>();

        // Give the player their loan at the start of the game
        if (state != null)
        {
            state.AddMoney(monthlyLoanIncome);
            Debug.Log($"Student loan received: +£{monthlyLoanIncome}");
        }
    }

    public void OnNewDay()
    {
        totalDaysPassed++;
        daysUntilRentDue--;

        // After rent day passes, reset countdown for next monthly cycle
        if (daysUntilRentDue < 0)
        {
            daysUntilRentDue = daysPerMonth - 1;
        }

        // Give the next student loan every month
        if (totalDaysPassed > 0 && totalDaysPassed % daysPerMonth == 0)
        {
            if (state != null)
            {
                state.AddMoney(monthlyLoanIncome);
                Debug.Log($"New monthly student loan received: +£{monthlyLoanIncome}");
            }
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
        return state != null && state.money >= monthlyRent;
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