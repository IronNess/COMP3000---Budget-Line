using UnityEngine;
using TMPro;

public class CleaningGameController : MonoBehaviour
{
    [SerializeField] private int totalItems = 6;
    [SerializeField] private float timeLimit = 20f;

    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private TMP_Text resultText;

    private int cleanedItems = 0;
    private float timer;
    private bool gameEnded = false;

    private void Start()
    {
        timer = timeLimit;
        UpdateUI();

        if (resultText != null)
            resultText.gameObject.SetActive(false);
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

    public void ItemCleaned()
    {
        if (gameEnded) return;

        cleanedItems++;
        UpdateUI();

        if (cleanedItems >= totalItems)
        {
            EndGame(true);
        }
    }

    private void UpdateUI()
    {
        if (timerText != null)
            timerText.text = "Time: " + timer.ToString("F1");

        if (progressText != null)
            progressText.text = "Cleaned: " + cleanedItems + "/" + totalItems;
    }

    private void EndGame(bool won)
    {
        gameEnded = true;

        if (resultText != null)
        {
            resultText.gameObject.SetActive(true);
            resultText.text = won ? "Room Clean!" : "Time Up!";
        }
    }
}