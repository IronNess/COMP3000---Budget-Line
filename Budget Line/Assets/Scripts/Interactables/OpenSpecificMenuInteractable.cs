using UnityEngine;

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

    [SerializeField] private MenuController menuController;
    [SerializeField] private MenuType menuType;

    public string Prompt => "Open Menu";

    private void Awake()
    {
        if (!menuController)
            menuController = FindObjectOfType<MenuController>();
    }

    public void Interact()
    {
        if (menuController == null)
        {
            Debug.LogError("OpenSpecificMenuInteractable: MenuController not assigned.");
            return;
        }

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