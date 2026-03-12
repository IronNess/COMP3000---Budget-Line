using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ClickableInteractable : MonoBehaviour
{
    private IInteractable interactable;

    private void Awake()
    {
        interactable = GetComponent<IInteractable>();

        if (interactable == null)
        {
            Debug.LogError($"{gameObject.name} is missing an IInteractable component.");
        }
    }

    private void OnMouseDown()
    {
        if (interactable != null)
        {
            interactable.Interact();
        }
    }
}