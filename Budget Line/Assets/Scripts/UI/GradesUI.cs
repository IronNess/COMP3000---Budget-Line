// GradesUI.cs
using UnityEngine;
using TMPro;

/// <summary>
/// Updates a grade label when stats change. Shows the numeric score (0–100), same as <see cref="UIHud"/>.
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

        label.text = "Grade: " + state.GetGrades();
    }
}