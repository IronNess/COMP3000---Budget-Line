using UnityEngine;

public class DoorMenuInteractable : MonoBehaviour, IInteractable
{
    public string Prompt => "Use Door (Choose: Work or University)";

    [SerializeField] private TravelMenuUI travelMenu;

    private void Awake()
    {
        if (!travelMenu) travelMenu = FindObjectOfType<TravelMenuUI>(true);
    }

    public void Interact()
    {
        travelMenu.Open();
    }
}
