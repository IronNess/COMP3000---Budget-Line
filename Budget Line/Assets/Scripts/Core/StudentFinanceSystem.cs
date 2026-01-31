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

    [SerializeField] private GameState state;
    [SerializeField] private TimeSystem timeSystem;

    private int dayCounter = 0;

    private void Awake()
    {
        if (!state) state = FindObjectOfType<GameState>();
        if (!timeSystem) timeSystem = FindObjectOfType<TimeSystem>();

        
        ApplyMonthlyCycle(isFirstMonth: true);
    }

    private void Update()
    {
        // Detect day changes by tracking weekday wrap 
        // increment a counter each time TimeSystem wraps to Morning
    
    }

    /// <summary>
    /// Call this once per new day
    /// </summary>
    public void OnNewDay()
    {
        dayCounter++;

        if (dayCounter >= daysPerMonth)
        {
            dayCounter = 0;
            ApplyMonthlyCycle(isFirstMonth: false);
        }
    }

    private void ApplyMonthlyCycle(bool isFirstMonth)
    {
        int totalMonthlyCosts = rent + groceries + bills + transport + diningOut + clothingShopping;

        // Income
        state.AddMoney(monthlyLoanIncome);

        // Costs
        state.AddMoney(-totalMonthlyCosts);

        // Stress impact
        state.AddStress(+10);

        
        Debug.Log($"Monthly cycle applied. Income +£{monthlyLoanIncome}, Costs -£{totalMonthlyCosts}.");
    }

    public int GetTotalMonthlyCosts()
    {
        return rent + groceries + bills + transport + diningOut + clothingShopping;
    }
}
