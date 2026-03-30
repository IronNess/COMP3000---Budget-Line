using UnityEngine;

public class EndingSystem : MonoBehaviour
{
    [SerializeField] private GameState state;
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private EndGameUI endGameUI;

    [SerializeField] private int endDayThreshold = 28;

    private bool gameEnded = false;

    private void Awake()
    {
        if (!state) state = FindObjectOfType<GameState>();
        if (!timeSystem) timeSystem = FindObjectOfType<TimeSystem>();
        if (!endGameUI) endGameUI = FindObjectOfType<EndGameUI>();
    }

    private void Update()
    {
        if (gameEnded || timeSystem == null || state == null || endGameUI == null)
            return;

        // Early fail conditions
        if (state.stress >= 100)
        {
            EndGameNow("Burnout");
            return;
        }

        if (state.money < -200)
        {
            EndGameNow("Debt Collapse");
            return;
        }

        if (state.grades <= 10 && timeSystem.totalDaysPassed > 5)
        {
            EndGameNow("Academic Failure");
            return;
        }

        // End of game at day threshold
        if (timeSystem.totalDaysPassed >= endDayThreshold)
        {
            if (state.money > 100 && state.stress < 50 && state.grades > 5)
                EndGameNow("Ending: Stability");
            else if (state.money >= 0 && state.stress < 80)
                EndGameNow("Ending: Survival");
            else
                EndGameNow("Ending: Collapse");
        }
    }

    private void EndGameNow(string title)
    {
        if (gameEnded) return;

        gameEnded = true;
        Debug.Log("END GAME NOW: " + title);

        endGameUI.ShowEndScreen(title);

        enabled = false;
    }
}