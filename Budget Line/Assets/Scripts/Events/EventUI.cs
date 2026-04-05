using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject panel;
    public TMP_Text titleText;
    public TMP_Text bodyText;
    public Button[] choiceButtons;

    private EventData current;

    [SerializeField] private GameState state;
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private EventData libraryContestReason;

    private void Awake()
    {
        if (!state) state = FindObjectOfType<GameState>();
        if (!timeSystem) timeSystem = FindObjectOfType<TimeSystem>();

        if (panel) panel.SetActive(false);
    }

    public void Show(EventData data)
    {
        current = data;
        panel.SetActive(true);

        titleText.text = data.title;
        bodyText.text = data.description;

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < data.choices.Length)
            {
                int idx = i;
                choiceButtons[i].gameObject.SetActive(true);
                choiceButtons[i].GetComponentInChildren<TMP_Text>().text = data.choices[i].label;

                choiceButtons[i].onClick.RemoveAllListeners();
                choiceButtons[i].onClick.AddListener(() => Choose(idx));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void Choose(int index)
    {
        var c = current.choices[index];

        if (c.nextEventOverride != null)
        {
            Show(c.nextEventOverride);
            return;
        }

        if (current.title == "Library Fine" && c.label == "Contest" && libraryContestReason != null)
        {
            Show(libraryContestReason);
            return;
        }

        state.AddMoney(c.moneyDelta);
        state.AddEnergy(c.energyDelta);
        state.AddStress(c.stressDelta);
        state.AddHunger(c.hungerDelta);
        state.AddHygiene(c.hygieneDelta);
        state.AddGrades(c.gradesDelta);

        if (c.timeBlocksCost > 0)
            timeSystem.AdvanceTime(c.timeBlocksCost);

        panel.SetActive(false);
    }

    public void ShowCustom(string title, string message)
    {
        panel.SetActive(true);

        titleText.text = title;
        bodyText.text = message;

        foreach (var btn in choiceButtons)
        {
            btn.gameObject.SetActive(false);
        }
    }
}

