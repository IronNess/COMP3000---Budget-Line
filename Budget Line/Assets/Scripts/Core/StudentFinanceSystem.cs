using UnityEngine;

public class StudentFinanceSystem : MonoBehaviour
{
    [Header("Real UK student budget (Save the Student)")]
    public int monthlyLoanIncome = 750;

    public int rent = 529;
    public int groceries = 146;
    public int bills = 69;
    public int transport = 67;
    public int diningOut = 49;
    public int clothingShopping = 40;

    [Header("How many days count as a 'month' in-game")]
    public int daysPerMonth = 28; // simple 4-week month

    [Header("References")]
    [SerializeField] private GameState state;
    [SerializeField] private TimeSystem timeSystem;

    private int dayCounter = 0;

    private void Awake()
    {
        if (!state) state = FindObjectOfType<GameState>();
        if (!timeSystem) timeSystem = FindObjectOfType<TimeSystem>();

        ApplyMonthlyCycle(true);
    }

    public void OnNewDay()
    {
        dayCounter++;

        if (dayCounter >= daysPerMonth)
        {
            dayCounter = 0;
            ApplyMonthlyCycle(false);
        }
    }

    private void ApplyMonthlyCycle(bool isFirstMonth)
    {
        int totalMonthlyCosts = rent + groceries + bills + transport + diningOut + clothingShopping;

        state.AddMoney(monthlyLoanIncome);
        state.AddMoney(-totalMonthlyCosts);
        state.AddStress(+10);

        Debug.Log($"Monthly cycle applied. Income +£{monthlyLoanIncome}, Costs -£{totalMonthlyCosts}.");
    }

    public int GetTotalMonthlyCosts()
    {
        return rent + groceries + bills + transport + diningOut + clothingShopping;
    }
}