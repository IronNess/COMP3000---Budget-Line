using UnityEngine;

/// <summary>
/// Handles chained follow-up consequences that emerge from ongoing player conditions,
/// such as rent trouble, burnout risk, hunger problems, and hygiene problems.
/// 
/// Why this is better:
/// - SRP: only consequence-chain logic lives here.
/// - DRY: daily checks route through one method and helper methods.
/// - YAGNI: avoids building a full rule engine for a small set of consequence chains.
/// </summary>
public class ConsequenceChainSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameState state;
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private UniversalPopupUI popupUI;

    private WeekDay lastDayChecked;

    private void Awake()
    {
        ResolveReferences();

        if (timeSystem != null)
        {
            lastDayChecked = timeSystem.day;
        }
    }

    private void Update()
    {
        if (state == null || timeSystem == null || popupUI == null)
            return;

        if (timeSystem.day != lastDayChecked)
        {
            lastDayChecked = timeSystem.day;
            CheckDailyChains();
        }
    }

    private void ResolveReferences()
    {
        if (state == null) state = FindObjectOfType<GameState>();
        if (timeSystem == null) timeSystem = FindObjectOfType<TimeSystem>();
        if (popupUI == null) popupUI = FindObjectOfType<UniversalPopupUI>();
    }

    /// <summary>
    /// Evaluates daily consequence chains in priority order.
    /// Returns after the first triggered consequence to avoid stacking multiple popups at once.
    /// </summary>
    private void CheckDailyChains()
    {
        if (TryShowLandlordWarning()) return;
        if (TryShowEvictionRisk()) return;
        if (TryShowBurnoutWarning()) return;
        if (TryShowSkippingMealsWarning()) return;
        TryShowHygieneWarning();
    }

    private bool TryShowLandlordWarning()
    {
        if (!state.rentMissed || state.landlordWarningShown)
            return false;

        popupUI.Show(
            "Landlord Warning",
            "Your landlord has issued a warning due to unpaid rent. Another missed payment may have serious consequences.",
            null,
            "OK",
            () => { }
        );

        state.landlordWarningShown = true;
        state.AddStress(+15);
        return true;
    }

    private bool TryShowEvictionRisk()
    {
        if (!state.rentMissed || !state.landlordWarningShown || state.evictionRiskShown || state.GetMoney() >= 0)
            return false;

        popupUI.Show(
            "Eviction Risk",
            "You are at risk of losing your accommodation if your debt continues to grow.",
            null,
            "OK",
            () => { }
        );

        state.evictionRiskShown = true;
        state.AddStress(+20);
        return true;
    }

    private bool TryShowBurnoutWarning()
    {
        if (!state.burnoutRisk)
            return false;

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
        return true;
    }

    private bool TryShowSkippingMealsWarning()
    {
        if (state.GetHunger() > 10)
            return false;

        popupUI.Show(
            "Skipping Meals",
            "You have been skipping meals too often. Your health and focus are starting to suffer.",
            null,
            "OK",
            () => { }
        );

        state.AddEnergy(-10);
        state.AddStress(+10);
        return true;
    }

    private bool TryShowHygieneWarning()
    {
        if (state.GetHygiene() > 10)
            return false;

        popupUI.Show(
            "Hygiene Consequences",
            "Neglecting self-care is beginning to affect your confidence and wellbeing.",
            null,
            "OK",
            () => { }
        );

        state.AddStress(+10);
        return true;
    }
}