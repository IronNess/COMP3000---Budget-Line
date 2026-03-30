using UnityEngine;

public class PlayerInteraction3D : MonoBehaviour
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
            if (hud != null)
                hud.SetPrompt($"Press E to: {current.Prompt}");

            if (Input.GetKeyDown(interactKey))
                current.Interact();
        }
        else
        {
            if (hud != null)
                hud.SetPrompt("");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
            current = interactable;
    }

    private void OnTriggerExit(Collider other)
    {
        var interactable = other.GetComponent<IInteractable>();
        if (interactable != null && current == interactable)
            current = null;
    }
}