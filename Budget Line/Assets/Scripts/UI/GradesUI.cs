using UnityEngine;
using TMPro;

public class GradesUI : MonoBehaviour
{
    public GameState state;
    public TextMeshProUGUI label;

    private void Start()
    {
        if (!state)
            state = FindObjectOfType<GameState>();

        if (state != null)
            state.OnStatsChanged += UpdateGrades;

        UpdateGrades();
    }

    private void OnDestroy()
    {
        if (state != null)
            state.OnStatsChanged -= UpdateGrades;
    }

    private void UpdateGrades()
    {
        if (label == null || state == null) return;

        int score = state.GetGrades();

        string grade;

        if (score >= 80)
            grade = "A";
        else if (score >= 60)
            grade = "B";
        else if (score >= 40)
            grade = "C";
        else
            grade = "D";

        label.text = "Grade: " + grade;
    }
}