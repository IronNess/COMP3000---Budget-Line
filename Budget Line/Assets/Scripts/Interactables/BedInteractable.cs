using UnityEngine;

public class BedInteractable : MonoBehaviour, IInteractable
{
    public string Prompt => "Sleep (+Energy, -Stress, time passes, -Hunger)";

    [SerializeField] private GameState state;
    [SerializeField] private TimeSystem timeSystem;

    private void Awake()
    {
        if (!state) state = FindObjectOfType<GameState>();
        if (!timeSystem) timeSystem = FindObjectOfType<TimeSystem>();
    }

    public void Interact()
    {
        state.AddEnergy(+35);
        state.AddStress(-10);
        state.AddHunger(-10); // cost
        timeSystem.AdvanceTime(2);
    }
}

public class ClickDebug : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log("Bed clicked");
    }
}
