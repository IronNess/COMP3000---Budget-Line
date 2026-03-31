using UnityEngine;

/// <summary>
/// Laundry interaction:
/// presents washing options and applies the chosen result.
/// 
/// Why this is better:
/// - SRP: handles only laundry-related decisions.
/// - DRY: one ApplyWash(...) method handles both options.
/// - Big improvement: removes repeated FindObjectOfType calls from every wash action.
/// </summary>
public class LaundryInteractable : MonoBehaviour, IInteractable
{
    public string Prompt => "Do Laundry";

    [Header("References")]
    [SerializeField] private InteractionChoiceUI choiceUI;
    [SerializeField] private GameState state;
    [SerializeField] private TimeSystem timeSystem;

    [Header("Quick Wash")]
    [SerializeField] private int quickWashEnergyCost = 5;
    [SerializeField] private int quickWashHygieneGain = 10;
    [SerializeField] private int quickWashTime = 1;

    [Header("Full Wash")]
    [SerializeField] private int fullWashEnergyCost = 10;
    [SerializeField] private int fullWashHygieneGain = 25;
    [SerializeField] private int fullWashTime = 2;

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
            "Laundry",
            "Your clothes are piling up.",
            new InteractionChoice("Quick Wash", () =>
                ApplyWash(quickWashEnergyCost, quickWashHygieneGain, quickWashTime)),
            new InteractionChoice("Full Wash", () =>
                ApplyWash(fullWashEnergyCost, fullWashHygieneGain, fullWashTime))
        );
    }

    private void ApplyWash(int energyCost, int hygieneGain, int timeCost)
    {
        state.AddEnergy(-energyCost);
        state.AddHygiene(hygieneGain);
        timeSystem.AdvanceTime(timeCost);
    }
}