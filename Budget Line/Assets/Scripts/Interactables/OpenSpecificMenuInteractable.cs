using UnityEngine;

/// <summary>
/// Opens one specific menu from the MenuController.
/// 
/// Why this is better:
/// - SRP: only routes to the chosen menu.
/// - DRY: switch is delegated to one helper method.
/// - YAGNI: enum + switch is enough here; no need for a larger menu-command system.
/// </summary>
public class OpenSpecificMenuInteractable : MonoBehaviour, IInteractable
{
    public enum MenuType
    {
        Travel,
        Meal,
        Hygiene,
        Study,
        Work
    }

    public string Prompt => "Open Menu";

    [SerializeField] private MenuController menuController;
    [SerializeField] private MenuType menuType;

    private void Awake()
    {
        if (menuController == null)
            menuController = FindObjectOfType<MenuController>();
    }

    public void Interact()
    {
        if (menuController == null)
        {
            Debug.LogError("OpenSpecificMenuInteractable: MenuController not assigned.");
            return;
        }

        OpenConfiguredMenu();
    }

    private void OpenConfiguredMenu()
    {
        switch (menuType)
        {
            case MenuType.Travel:
                menuController.OpenTravelMenu();
                break;
            case MenuType.Meal:
                menuController.OpenMealMenu();
                break;
            case MenuType.Hygiene:
                menuController.OpenHygieneMenu();
                break;
            case MenuType.Study:
                menuController.OpenStudyMenu();
                break;
            case MenuType.Work:
                menuController.OpenWorkMenu();
                break;
        }
    }
}