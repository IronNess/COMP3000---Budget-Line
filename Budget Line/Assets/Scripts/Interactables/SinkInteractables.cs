using UnityEngine;

public class SinkInteractable : MonoBehaviour, IInteractable
{
    public string Prompt => "Wash (+Hygiene, time passes)";

    [SerializeField] private GameState state;
    [SerializeField] private TimeSystem timeSystem;

    private void Awake()
    {
        if (!state) state = FindObjectOfType<GameState>();
        if (!timeSystem) timeSystem = FindObjectOfType<TimeSystem>();
    }

    public void Interact()
    {
        state.AddHygiene(+25);
        timeSystem.AdvanceTime(1);
    }
}
