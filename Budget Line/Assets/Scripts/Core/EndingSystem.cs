using UnityEngine;

/// <summary>
/// Evaluates fail states and successful ending states.
/// 
/// Caller:
/// - Unity Update loop checks end conditions continuously.
/// - Calls EndGameUI when a final state is reached.
/// 
/// Why this is better:
/// - SRP: only ending-condition logic lives here.
/// - DRY: all endings route through EndGameNow(...).
/// - YAGNI: does not introduce a complex state machine because current scope does not require it.
/// </summary>
public class EndingSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameState state;
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private EndGameUI endGameUI;

    [Header("Failure Thresholds")]
    [SerializeField] private int endDayThreshold = 28;
    [SerializeField] private int debtFailureThreshold = -200;
    [SerializeField] private int academicFailureThreshold = 10;
    [SerializeField] private int minimumDaysBeforeAcademicFailure = 5;

    private bool gameEnded = false;

    private void Awake()
    {
        ResolveReferences();
    }

    private void ResolveReferences()
    {
        if (state == null) state = FindObjectOfType<GameState>();
        if (timeSystem == null) timeSystem = FindObjectOfType<TimeSystem>();
        if (endGameUI == null) endGameUI = FindObjectOfType<EndGameUI>();
    }

    private void Update()
    {
        if (gameEnded || state == null || timeSystem == null || endGameUI == null)
            return;

        if (TryTriggerFailureEnding()) return;
        TryTriggerFinalEnding();
    }

    /// <summary>
    /// Checks early fail conditions first.
    /// DRY: failure logic is centralised into one method.
    /// </summary>
    private bool TryTriggerFailureEnding()
    {
        if (state.GetStress() >= 100)
        {
            EndGameNow("Burnout");
            return true;
        }

        if (state.GetMoney() < debtFailureThreshold)
        {
            EndGameNow("Debt Collapse");
            return true;
        }

        if (state.GetGrades() <= academicFailureThreshold &&
            timeSystem.totalDaysPassed > minimumDaysBeforeAcademicFailure)
        {
            EndGameNow("Academic Failure");
            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks the standard end-of-game result after enough days have passed.
    /// </summary>
    private void TryTriggerFinalEnding()
    {
        if (timeSystem.totalDaysPassed < endDayThreshold)
            return;

        if (state.GetMoney() > 100 && state.GetStress() < 50 && state.GetGrades() > 5)
        {
            EndGameNow("Ending: Stability");
        }
        else if (state.GetMoney() >= 0 && state.GetStress() < 80)
        {
            EndGameNow("Ending: Survival");
        }
        else
        {
            EndGameNow("Ending: Collapse");
        }
    }

    /// <summary>
    /// Shared exit point for all end states.
    /// DRY: one path guarantees all endings behave consistently.
    /// </summary>
    private void EndGameNow(string title)
    {
        if (gameEnded) return;

        gameEnded = true;
        Debug.Log("END GAME NOW: " + title);

        endGameUI.ShowEndScreen(title);
        enabled = false;
    }
}