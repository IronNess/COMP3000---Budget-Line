using UnityEngine;

public class DebtLateFeeTrigger : MonoBehaviour
{
    public EventData lateFeeEvent;
    public int debtThreshold = -20;
    public int minDaysBetweenTriggers = 2;

    private int daysSinceLastTrigger = 999;

    [SerializeField] private GameState state;
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private EventUI eventUI;

    private WeekDay lastDay;

    private void Awake()
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
            lastDay = timeSystem.day;
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
