using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CleaningGameController : MonoBehaviour
{
    [Header("Game Rules")]
    [SerializeField] private float timeLimit = 20f;

    [Header("Scene Flow")]
    [SerializeField] private string returnSceneName = "RoomScene";

    [Header("UI")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text progressText;

    [Header("Optional Reward Hook")]
    [SerializeField] private GameState gameState;
    [SerializeField] private int hygieneRewardAmount = 1000; 
    // Assumes AddHygiene clamps to 100 in GameState.

    private int totalItems;
    private int cleanedItems;
    private float timer;
    private bool gameEnded;

    public bool IsGameEnded => gameEnded;

    private void Start()
    {
        timer = timeLimit;
        UpdateUI();
    }

    private void Update()
    {
        if (gameEnded) return;

        timer -= Time.deltaTime;
        if (timer < 0f) timer = 0f;

        UpdateUI();

        if (timer <= 0f)
        {
            EndGame(false);
        }
    }

    public void RegisterItem()
    {
        totalItems++;
        UpdateUI();
    }

    public void ItemCleaned()
    {
        if (gameEnded) return;

        cleanedItems++;
        UpdateUI();

        if (cleanedItems >= totalItems && totalItems > 0)
        {
            EndGame(true);
        }
    }

    public void ExitCleaningGame()
    {
        if (gameEnded) return;

        gameEnded = true;
        LoadReturnScene();
    }

    private void UpdateUI()
    {
        if (timerText != null)
            timerText.text = $"Time: {timer:F1}";

        if (progressText != null)
            progressText.text = $"Cleaned: {cleanedItems}/{totalItems}";
    }

    private void EndGame(bool won)
    {
        if (gameEnded) return;

        gameEnded = true;

        if (won && gameState != null)
        {
            gameState.AddHygiene(hygieneRewardAmount);
        }

        LoadReturnScene();
    }

    private void LoadReturnScene()
    {
        SceneManager.LoadScene(returnSceneName);
    }
}