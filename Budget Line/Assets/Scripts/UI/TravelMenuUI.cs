using UnityEngine;

public class TravelMenuUI : MonoBehaviour
{
    public GameObject panel;

    [SerializeField] private DoorUniversityInteractable uni;
    [SerializeField] private DoorWorkInteractable work;

    private void Awake()
    {
        if (!panel) panel = gameObject;
        panel.SetActive(false);

        if (!uni) uni = FindObjectOfType<DoorUniversityInteractable>();
        if (!work) work = FindObjectOfType<DoorWorkInteractable>();
    }

    public void Open()
    {
        panel.SetActive(true);
        Time.timeScale = 0f; // pause while choosing
    }

    public void Close()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }

    // Hook these two methods to the UI Buttons OnClick
    public void ChooseUniversity()
    {
        Close();
        uni.Interact();
    }

    public void ChooseWork()
    {
        Close();
        work.Interact();
    }
}
