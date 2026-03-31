// GradesUI.cs
using UnityEngine;
using TMPro;

/// <summary>
/// Updates a grade label when stats change.
/// 
/// - DRY: uses GradeUtility instead of repeating grade rules.
/// - SRP: this class only updates one UI label.
/// </summary>
public class GradesUI : MonoBehaviour
{
    [SerializeField] private GameState state;
    [SerializeField] private TextMeshProUGUI label;

    private void Awake()
    {
        if (state == null) state = FindObjectOfType<GameState>();
    }

    private void OnEnable()
    {
        if (state != null)
        {
            state.OnStatsChanged += UpdateGrades;
        }

        UpdateGrades();
    }

    private void OnDisable()
    {
        if (state != null)
        {
            state.OnStatsChanged -= UpdateGrades;
        }
    }

    private void UpdateGrades()
    {
        if (label == null || state == null) return;

        label.text = "Grade: " + GradeUtility.ToLetter(state.GetGrades());
    }
}