// TravelMenuUI.cs
using UnityEngine;

/// <summary>
/// Controls the travel menu and forwards button choices to the correct gameplay actions.
/// 
/// Caller:
/// UI Buttons call the public Choose... methods.
/// 
/// Why this is better:
/// - SRP: handles travel menu presentation and forwarding only.
/// - DRY: all option methods reuse Close() and ResolvePlayerActions().
/// - YAGNI: keeps direct button methods because that is the current project need.
/// </summary>
public class TravelMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private DoorUniversityInteractable uni;
    [SerializeField] private DoorWorkInteractable work;

    private PlayerActions cachedPlayerActions;

    private void Awake()
    {
        if (panel == null) panel = gameObject;
        if (uni == null) uni = FindObjectOfType<DoorUniversityInteractable>();
        if (work == null) work = FindObjectOfType<DoorWorkInteractable>();

        panel.SetActive(false);
    }

    public void Open()
    {
        panel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Close()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ChooseUniversity()
    {
        Close();

        if (uni != null)
        {
            uni.Interact();
        }
    }

    public void ChooseWork()
    {
        Close();

        if (work != null)
        {
            work.Interact();
        }
    }

    public void ChooseGym()
    {
        Close();

        PlayerActions actions = ResolvePlayerActions();
        if (actions != null)
        {
            actions.GoToGym();
        }
    }

    public void ChooseDowntown()
    {
        Close();

        PlayerActions actions = ResolvePlayerActions();
        if (actions != null)
        {
            actions.GoDowntown();
        }
    }

    private PlayerActions ResolvePlayerActions()
    {
        if (cachedPlayerActions == null)
        {
            cachedPlayerActions = FindObjectOfType<PlayerActions>();
        }

        return cachedPlayerActions;
    }
}