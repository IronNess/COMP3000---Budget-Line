using UnityEngine;

public class FridgeInteractable : MonoBehaviour, IInteractable
{
    public string Prompt => "Eat (-£5, +Hunger, time passes)";

    public int mealCost = 5;
    public int hungerGain = 25;

    [SerializeField] private GameState state;
    [SerializeField] private TimeSystem timeSystem;

    private void Awake()
    {
        if (!state) state = FindObjectOfType<GameState>();
        if (!timeSystem) timeSystem = FindObjectOfType<TimeSystem>();
    }

    public void Interact()
    {
        // Soft lock: can't eat if you can't afford it
        if (state.money < mealCost)
        {
            state.AddStress(+2);            // small penalty
            // Optional: show a popup if you want later
            return;
        }

        state.AddMoney(-mealCost);
        state.AddHunger(+hungerGain);
        timeSystem.AdvanceTime(1);
    }
}
