using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))]
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

    private void OnMouseUpAsButton()
    {
  
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (interactable != null)
        {
            interactable.Interact();
        }
    }
}