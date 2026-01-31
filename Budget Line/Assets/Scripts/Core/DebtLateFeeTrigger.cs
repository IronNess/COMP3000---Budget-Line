using UnityEngine;

public class DebtLateFeeTrigger : MonoBehaviour
{
    public EventData lateFeeEvent; // Variables
    public int debtThreshold = -20; // The money value that counts as "in debt"
    public int minDaysBetweenTriggers = 2; // Minimum number of days between late fees

    private int daysSinceLastTrigger = 999; // Counter for number of days since last late fee

    [SerializeField] private GameState state;
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private EventUI eventUI;

    private WeekDay lastDay; // Day tracking

    private void Awake() // Stepup phase
    {
        if (!state) state = FindObjectOfType<GameState>();
        if (!timeSystem) timeSystem = FindObjectOfType<TimeSystem>();
        if (!eventUI) eventUI = FindObjectOfType<EventUI>();

        lastDay = timeSystem.day;
    }

    private void Update()
    {
        // detect day changes
        if (timeSystem.day != lastDay)
        {
            lastDay = timeSystem.day; // Update scored day, increases the "days since last late fee"
            daysSinceLastTrigger++;

            if (lateFeeEvent != null &&
                daysSinceLastTrigger >= minDaysBetweenTriggers &&
                state.money <= debtThreshold)
            {
                daysSinceLastTrigger = 0;
                eventUI.Show(lateFeeEvent);
            }
        }
    }
}
