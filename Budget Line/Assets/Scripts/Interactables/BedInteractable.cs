using UnityEngine;

/// <summary>
/// Bed interaction:
/// restores energy, reduces stress, costs hunger, and advances time.
/// 
/// Caller:
/// ClickableInteractable calls Interact() through IInteractable.
/// 
/// Why this is better:
/// - SRP: this class only defines the bed action.
/// - DRY: dependency resolution is centralised in ResolveReferences().
/// - YAGNI: no unnecessary abstraction beyond what this interactable needs.
/// </summary>
public class BedInteractable : MonoBehaviour, IInteractable
{
    public string Prompt => "Sleep (+Energy, -Stress, time passes, -Hunger)";

    [Header("Bed Effects")]
    [SerializeField] private int energyGain = 35;
    [SerializeField] private int stressReduction = 10;
    [SerializeField] private int hungerCost = 10;
    [SerializeField] private int timeCost = 2;

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

        state.AddEnergy(energyGain);
        state.AddStress(-stressReduction);
        state.AddHunger(-hungerCost);
        timeSystem.AdvanceTime(timeCost);
    }
}