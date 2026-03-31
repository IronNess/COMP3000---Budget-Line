using UnityEngine;

/// <summary>
/// Controls opening and closing the main gameplay menus.
/// 
/// Caller:
/// - OpenSpecificMenuInteractable
/// - UI buttons
/// 
/// Why this is better:
/// - SRP/SOLID: only handles menu visibility.
/// - DRY: all menu closing logic is centralised in one place.
/// - YAGNI: does not introduce an unnecessary menu framework.
/// </summary>
public class MenuController : MonoBehaviour
{
    [Header("Menu References")]
    [SerializeField] private GameObject travelMenu;
    [SerializeField] private GameObject mealMenu;
    [SerializeField] private GameObject hygieneMenu;
    [SerializeField] private GameObject studyMenu;
    [SerializeField] private GameObject workMenu;

    /// <summary>
    /// Hides all menus.
    /// DRY: shared close logic avoids repeating SetActive(false) patterns everywhere.
    /// </summary>
    public void CloseAllMenus()
    {
        SetMenuActive(travelMenu, false);
        SetMenuActive(mealMenu, false);
        SetMenuActive(hygieneMenu, false);
        SetMenuActive(studyMenu, false);
        SetMenuActive(workMenu, false);
    }

    public void OpenTravelMenu() => OpenMenu(travelMenu);
    public void OpenMealMenu() => OpenMenu(mealMenu);
    public void OpenHygieneMenu() => OpenMenu(hygieneMenu);
    public void OpenStudyMenu() => OpenMenu(studyMenu);
    public void OpenWorkMenu() => OpenMenu(workMenu);

    /// <summary>
    /// Opens one menu after closing all others.
    /// </summary>
    private void OpenMenu(GameObject menuToOpen)
    {
        CloseAllMenus();
        SetMenuActive(menuToOpen, true);
    }

    /// <summary>
    /// Small helper to avoid duplicated null checks.
    /// </summary>
    private void SetMenuActive(GameObject targetMenu, bool isActive)
    {
        if (targetMenu != null)
        {
            targetMenu.SetActive(isActive);
        }
    }
}