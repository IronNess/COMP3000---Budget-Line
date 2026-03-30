using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MissingWordGameController : MonoBehaviour
{
    [System.Serializable]
    public class QuestionData
    {
        public string sentence;
        public string[] options;
        public int correctIndex;
    }

    [SerializeField] private TMP_Text questionText;
    [SerializeField] private Button[] optionButtons;
    [SerializeField] private TMP_Text[] optionTexts;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private Button nextButton;

    [SerializeField] private QuestionData[] questions;

    private int currentQuestion = 0;
    private int score = 0;

    private void Start()
    {
        if (resultText != null)
            resultText.text = "";

        if (nextButton != null)
            nextButton.gameObject.SetActive(false);

        ShowQuestion();
    }

    private void ShowQuestion()
    {
        if (currentQuestion >= questions.Length)
        {
            EndGame();
            return;
        }

        QuestionData q = questions[currentQuestion];
        questionText.text = q.sentence;

        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i;
            optionButtons[i].interactable = true;
            optionTexts[i].text = q.options[i];
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => ChooseAnswer(index));
        }

        if (nextButton != null)
            nextButton.gameObject.SetActive(false);

        UpdateScore();
    }

    public void ChooseAnswer(int index)
    {
        QuestionData q = questions[currentQuestion];

        for (int i = 0; i < optionButtons.Length; i++)
            optionButtons[i].interactable = false;

        if (index == q.correctIndex)
        {
            score++;
            resultText.text = "Correct!";
        }
        else
        {
            resultText.text = "Wrong!";
        }

        if (nextButton != null)
            nextButton.gameObject.SetActive(true);

        UpdateScore();
    }

    public void NextQuestion()
    {
        currentQuestion++;
        resultText.text = "";
        ShowQuestion();
    }

    private void UpdateScore()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    private void EndGame()
    {
        questionText.text = "Finished!";
        resultText.text = "Final Score: " + score + "/" + questions.Length;

        foreach (Button b in optionButtons)
            b.gameObject.SetActive(false);

        if (nextButton != null)
            nextButton.gameObject.SetActive(false);
    }
}