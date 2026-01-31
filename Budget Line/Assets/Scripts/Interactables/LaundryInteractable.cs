using UnityEngine;

public class LaundryInteractable : MonoBehaviour, IInteractable
{
    public string Prompt => "Do Laundry";

    [SerializeField] private InteractionChoiceUI choiceUI;

    public void Interact()
    {
        choiceUI.Show(
            "Laundry",
            "Your clothes are piling up.",
            new InteractionChoice("Quick Wash", () =>
            {
                Apply(-5, +10, 1);
            }),
            new InteractionChoice("Full Wash", () =>
            {
                Apply(-10, +25, 2);
            })
        );
    }

    private void Apply(int energyCost, int hygieneGain, int time)
    {
        var state = FindObjectOfType<GameState>();
        var timeSystem = FindObjectOfType<TimeSystem>();

        state.AddEnergy(energyCost);
        state.AddHygiene(hygieneGain);
        timeSystem.AdvanceTime(time);
    }
}
