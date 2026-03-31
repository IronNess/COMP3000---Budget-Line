// InteractionChoiceUI.cs
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Small reusable choice popup used by interactable objects.
/// 
/// Caller:
/// Other interactable scripts call Show(...) and pass button choices.
/// 

/// - SRP: only handles displaying and wiring choices.
/// - DRY: button setup is delegated to a helper method.
/// - Important bug fix: avoids capturing the loop variable incorrectly.
/// </summary>
public class InteractionChoiceUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text body;
    [SerializeField] private Button[] buttons;

    public void Show(string popupTitle, string popupBody, params InteractionChoice[] choices)
    {
        if (panel == null || title == null || body == null || buttons == null) return;

        panel.SetActive(true);
        title.text = popupTitle;
        body.text = popupBody;

        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < choices.Length)
            {
                ConfigureButton(buttons[i], choices[i]);
            }
            else
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
    }

    private void ConfigureButton(Button button, InteractionChoice choice)
    {
        if (button == null || choice == null) return;

        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();

        button.gameObject.SetActive(true);
        if (buttonText != null)
        {
            buttonText.text = choice.label;
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            choice.action?.Invoke();
            if (panel != null)
            {
                panel.SetActive(false);
            }
        });
    }
}

/// <summary>
/// Lightweight data object passed into InteractionChoiceUI.
/// YAGNI: only stores the two pieces of data the current UI needs.
/// </summary>
public class InteractionChoice
{
    public string label;
    public Action action;

    public InteractionChoice(string label, Action action)
    {
        this.label = label;
        this.action = action;
    }
}