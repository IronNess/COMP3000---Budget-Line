using UnityEngine;

/// <summary>
/// Opens the travel menu when the player uses the door.
/// 
/// Why this is better:
/// - SRP: this object only opens the menu.
/// - YAGNI: it does not contain travel logic itself.
/// </summary>
public class DoorMenuInteractable : MonoBehaviour, IInteractable
{
    public string Prompt => "Use Door (Choose: Work or University)";

    [SerializeField] private TravelMenuUI travelMenu;

    private void Awake()
    {
        if (travelMenu == null)
            travelMenu = FindObjectOfType<TravelMenuUI>(true);
    }

    public void Interact()
    {
        if (travelMenu != null)
            travelMenu.Open();
    }
}