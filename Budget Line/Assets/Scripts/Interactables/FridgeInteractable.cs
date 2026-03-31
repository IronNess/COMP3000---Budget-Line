using UnityEngine;

/// <summary>
/// Fridge interaction:
/// buys and eats a quick meal.
/// 
/// Why this is better:
/// - SRP: only handles fridge behaviour.
/// - DRY: one helper decides affordability.
/// - YAGNI: keeps the logic small and direct.
/// </summary>
public class FridgeInteractable : MonoBehaviour, IInteractable
{
    public string Prompt => "Eat (-£5, +Hunger, time passes)";

    [Header("Meal Settings")]
    [SerializeField] private int mealCost = 5;
    [SerializeField] private int hungerGain = 25;
    [SerializeField] private int timeCost = 1;
    [SerializeField] private int failedStressPenalty = 2;

    [Header("References")]
    [SerializeField] private GameState state;
    [SerializeField] private TimeSystem timeSystem;

    private void Awake()
    {
        ResolveReferences();
    }

    private void ResolveReferences()
    {
        if (state == null) state = FindObjectOfType<GameState>();
        if (timeSystem == null) timeSystem = FindObjectOfType<TimeSystem>();
    }

    public void Interact()
    {
        if (state == null || timeSystem == null) return;

        if (!CanAffordMeal())
        {
            state.AddStress(failedStressPenalty);
            return;
        }

        state.AddMoney(-mealCost);
        state.AddHunger(hungerGain);
        timeSystem.AdvanceTime(timeCost);
    }

    private bool CanAffordMeal()
    {
        return state.GetMoney() >= mealCost;
    }
}