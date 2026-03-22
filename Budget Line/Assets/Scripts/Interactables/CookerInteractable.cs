using UnityEngine;

public class CookerInteractable : MonoBehaviour
{
    [SerializeField] private InteractionChoiceUI choiceUI;
    [SerializeField] private GameState state;
    [SerializeField] private TimeSystem timeSystem;

    private void Awake()
    {
        if (!choiceUI) choiceUI = FindObjectOfType<InteractionChoiceUI>();
        if (!state) state = FindObjectOfType<GameState>();
        if (!timeSystem) timeSystem = FindObjectOfType<TimeSystem>();
    }

    public void Interact()
    {
        choiceUI.Show(
            "Cooker",
            "Cooking a meal takes longer, but restores much more hunger.",
            new InteractionChoice("Cook a meal", () =>
            {
                state.AddMoney(-15);
                state.AddHunger(+35);
                state.AddEnergy(-2);
                timeSystem.AdvanceTime(2);
            }),
            new InteractionChoice("Cancel", () => { })
        );
    }
}