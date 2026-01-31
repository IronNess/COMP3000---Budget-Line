using UnityEngine;

public class PlayerInteraction2D : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E;
    private IInteractable current;

    [SerializeField] private UIHud hud;

    private void Awake()
    {
        if (!hud) hud = FindObjectOfType<UIHud>();
    }

    private void Update()
    {
        if (current != null)
        {
            hud.SetPrompt($"Press E to: {current.Prompt}");

            if (Input.GetKeyDown(interactKey))
                current.Interact();
        }
        else
        {
            hud.SetPrompt("");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
            current = interactable;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var interactable = other.GetComponent<IInteractable>();
        if (interactable != null && current == interactable)
            current = null;
    }
}
