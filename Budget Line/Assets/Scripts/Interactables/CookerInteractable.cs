using UnityEngine;

/// <summary>
/// Cooker interaction:
/// shows a choice menu for cooking a meal.
/// 
/// Why this is better:
/// - SOLID/SRP: this script only manages cooker behaviour.
/// - DRY: cooking effects are routed through one ApplyCookMeal() method.
/// - YAGNI: keeps a simple choice-based flow without building a larger action system.
/// </summary>
public class CookerInteractable : MonoBehaviour, IInteractable
{
    public string Prompt => "Use Cooker";

    [Header("Cooker Settings")]
    [SerializeField] private int mealCost = 15;
    [SerializeField] private int hungerGain = 35;
    [SerializeField] private int energyCost = 2;
    [SerializeField] private int timeCost = 2;

    [Header("References")]
    [SerializeField] private InteractionChoiceUI choiceUI;
    [SerializeField] private GameState state;
    [SerializeField] private TimeSystem timeSystem;

    private void Awake()
    {
        ResolveReferences();
    }

    private void ResolveReferences()
    {
        if (choiceUI == null) choiceUI = FindObjectOfType<InteractionChoiceUI>();
        if (state == null) state = FindObjectOfType<GameState>();
        if (timeSystem == null) timeSystem = FindObjectOfType<TimeSystem>();
    }

    public void Interact()
    {
        if (choiceUI == null || state == null || timeSystem == null) return;

        choiceUI.Show(
            "Cooker",
            "Cooking a meal takes longer, but restores much more hunger.",
            new InteractionChoice("Cook a meal", ApplyCookMeal),
            new InteractionChoice("Cancel", () => { })
        );
    }

    private void ApplyCookMeal()
    {
        state.AddMoney(-mealCost);
        state.AddHunger(hungerGain);
        state.AddEnergy(-energyCost);
        timeSystem.AdvanceTime(timeCost);
    }
}