using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject travelMenu;
    public GameObject mealMenu;
    public GameObject hygieneMenu;
    public GameObject studyMenu;
    public GameObject workMenu;

    public void CloseAllMenus()
    {
        if (travelMenu) travelMenu.SetActive(false);
        if (mealMenu) mealMenu.SetActive(false);
        if (hygieneMenu) hygieneMenu.SetActive(false);
        if (studyMenu) studyMenu.SetActive(false);
        if (workMenu) workMenu.SetActive(false);
    }

    public void OpenTravelMenu()
    {
        CloseAllMenus();
        if (travelMenu) travelMenu.SetActive(true);
    }

    public void OpenMealMenu()
    {
        CloseAllMenus();
        if (mealMenu) mealMenu.SetActive(true);
    }

    public void OpenHygieneMenu()
    {
        CloseAllMenus();
        if (hygieneMenu) hygieneMenu.SetActive(true);
    }

    public void OpenStudyMenu()
    {
        CloseAllMenus();
        if (studyMenu) studyMenu.SetActive(true);
    }

    public void OpenWorkMenu()
    {
        CloseAllMenus();
        if (workMenu) workMenu.SetActive(true);
    }
}