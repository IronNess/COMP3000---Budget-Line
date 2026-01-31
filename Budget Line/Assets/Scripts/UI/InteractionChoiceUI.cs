using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class InteractionChoiceUI : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text title;
    public TMP_Text body;
    public Button[] buttons;

    public void Show(string t, string b, params InteractionChoice[] choices)
    {
        panel.SetActive(true);
        title.text = t;
        body.text = b;

        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < choices.Length)
            {
                buttons[i].gameObject.SetActive(true);
                buttons[i].GetComponentInChildren<TMP_Text>().text = choices[i].label;
                buttons[i].onClick.RemoveAllListeners();
                buttons[i].onClick.AddListener(() =>
                {
                    choices[i].action();
                    panel.SetActive(false);
                });
            }
            else buttons[i].gameObject.SetActive(false);
        }
    }
}

public class InteractionChoice
{
    public string label;
    public Action action;

    public InteractionChoice(string l, Action a)
    {
        label = l;
        action = a;
    }
}
