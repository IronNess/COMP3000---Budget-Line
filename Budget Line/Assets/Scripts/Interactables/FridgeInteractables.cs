using UnityEngine;

public class FridgeInteractable : MonoBehaviour, IInteractable
{
    public string Prompt => "Eat (-£5, +Hunger, time passes)";

    [SerializeField] private GameState state;
    [SerializeField] private TimeSystem timeSystem;

    private void Awake()
    {
        if (!state) state = FindObjectOfType<GameState>();
        if (!timeSystem) timeSystem = FindObjectOfType<TimeSystem>();
    }

    public void Interact()
    {
        state.AddMoney(-5);
        state.AddHunger(+25);
        timeSystem.AdvanceTime(1);
    }
}
