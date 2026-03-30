using UnityEngine;

public class EndingSystem : MonoBehaviour
{
    [SerializeField] private GameState state;
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private UniversalPopupUI popupUI;
    [SerializeField] private EndGameUI endGameUI;

    [SerializeField] private int endDayThreshold = 28;

    private bool gameEnded = false;

    private void Awake()
    {
        if (!state) state = FindObjectOfType<GameState>();
        if (!timeSystem) timeSystem = FindObjectOfType<TimeSystem>();
        if (!popupUI) popupUI = FindObjectOfType<UniversalPopupUI>();
        if (!endGameUI) endGameUI = FindObjectOfType<EndGameUI>();
    }

    private void Update()
    {
        if (gameEnded || timeSystem == null || state == null || endGameUI == null)
            return;

        // Immediate fail conditions
        if (state.stress >= 100)
        {
            ShowGameOver(
                "Burnout",
                "Your stress reached a critical level and you could no longer continue."
            );
            return;
        }

        if (state.money < -200)
        {
            ShowGameOver(
                "Debt Collapse",
                "Your debts spiralled too far out of control and you could no longer recover."
            );
            return;
        }

       if (state.grades <= 10)
{
    ShowGameOver(
        "Academic Failure",
        "Your academic performance declined too far and you were unable to keep up."
    );
    return;
}

        // Normal ending after enough days
        if (timeSystem.totalDaysPassed >= endDayThreshold)
        {
            ShowEnding();
            gameEnded = true;
        }
    }

    private void ShowEnding()
    {
        if (state.money > 100 && state.stress < 50 && state.grades > 5)
        {
            endGameUI.ShowEndScreen("Ending: Stability");
        }
        else if (state.money >= 0 && state.stress < 80)
        {
            endGameUI.ShowEndScreen("Ending: Survival");
        }
        else
        {
            endGameUI.ShowEndScreen("Ending: Collapse");
        }

        gameEnded = true;
    }

    private void ShowGameOver(string title, string body)
    {
        endGameUI.ShowEndScreen(title);
        gameEnded = true;
    }
}