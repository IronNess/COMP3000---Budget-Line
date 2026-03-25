using UnityEngine;

public class ConsequenceChainSystem : MonoBehaviour
{
    [SerializeField] private GameState state;
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private UniversalPopupUI popupUI;

    private WeekDay lastDayChecked;

    private void Awake()
    {
        if (!state) state = FindObjectOfType<GameState>();
        if (!timeSystem) timeSystem = FindObjectOfType<TimeSystem>();
        if (!popupUI) popupUI = FindObjectOfType<UniversalPopupUI>();

        if (timeSystem != null)
            lastDayChecked = timeSystem.day;
    }

    private void Update()
    {
        if (state == null || timeSystem == null || popupUI == null)
            return;

        // Only check once per new day
        if (timeSystem.day != lastDayChecked)
        {
            lastDayChecked = timeSystem.day;
            CheckDailyChains();
        }
    }

    private void CheckDailyChains()
    {
        // Rent chain: first warning
        if (state.rentMissed && !state.landlordWarningShown)
        {
            popupUI.Show(
                "Landlord Warning",
                "Your landlord has issued a warning due to unpaid rent. Another missed payment may have serious consequences.",
                null,
                "OK",
                () => { }
            );

            state.landlordWarningShown = true;
            state.AddStress(+15);
            return;
        }

        // Rent chain: eviction risk
        if (state.rentMissed && state.landlordWarningShown && !state.evictionRiskShown && state.money < 0)
        {
            popupUI.Show(
                "Eviction Risk",
                "You are at risk of losing your accommodation if your debt continues to grow.",
                null,
                "OK",
                () => { }
            );

            state.evictionRiskShown = true;
            state.AddStress(+20);
            return;
        }

        // Burnout follow-up
        if (state.burnoutRisk)
        {
            popupUI.Show(
                "Burnout Warning",
                "You have been pushing yourself too hard. Your exhaustion is starting to affect your ability to cope.",
                null,
                "OK",
                () => { }
            );

            state.AddEnergy(-10);
            state.AddStress(+10);
            state.burnoutRisk = false;
            return;
        }

        // Low hunger follow-up
        if (state.hunger <= 10)
        {
            popupUI.Show(
                "Skipping Meals",
                "You have been skipping meals too often. Your health and focus are starting to suffer.",
                null,
                "OK",
                () => { }
            );

            state.AddEnergy(-10);
            state.AddStress(+10);
            return;
        }

        // Low hygiene follow-up
        if (state.hygiene <= 10)
        {
            popupUI.Show(
                "Hygiene Consequences",
                "Neglecting self-care is beginning to affect your confidence and wellbeing.",
                null,
                "OK",
                () => { }
            );

            state.AddStress(+10);
        }
    }
}