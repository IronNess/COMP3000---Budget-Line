using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UniversalPopupUI : MonoBehaviour
{
    [Header("Panel")]
    public GameObject panel;
    public TMP_Text titleText;
    public TMP_Text bodyText;
    public Image iconImage;

    [Header("Buttons")]
    public Button button1;
    public TMP_Text button1Text;

    public Button button2;
    public TMP_Text button2Text;

    public Button button3;
    public TMP_Text button3Text;

    private void Awake()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    public void Show(
        string title,
        string body,
        Sprite icon = null,
        string btn1Label = null,
        Action btn1Action = null,
        string btn2Label = null,
        Action btn2Action = null,
        string btn3Label = null,
        Action btn3Action = null)
    {
        if (panel == null)
        {
            Debug.LogError("UniversalPopupUI: Panel is not assigned.");
            return;
        }

        panel.SetActive(true);

        if (titleText != null)
            titleText.text = title;

        if (bodyText != null)
            bodyText.text = body;

        if (iconImage != null)
        {
            if (icon != null)
            {
                iconImage.gameObject.SetActive(true);
                iconImage.sprite = icon;
            }
            else
            {
                iconImage.gameObject.SetActive(false);
            }
        }

        SetupButton(button1, button1Text, btn1Label, btn1Action);
        SetupButton(button2, button2Text, btn2Label, btn2Action);
        SetupButton(button3, button3Text, btn3Label, btn3Action);
    }

    private void SetupButton(Button button, TMP_Text label, string text, Action action)
    {
        if (button == null || label == null)
            return;

        button.onClick.RemoveAllListeners();

        if (string.IsNullOrEmpty(text))
        {
            button.gameObject.SetActive(false);
            return;
        }

        button.gameObject.SetActive(true);
        label.text = text;

        button.onClick.AddListener(() =>
        {
            action?.Invoke();
            Hide();
        });
    }

    public void Hide()
    {
        if (panel != null)
            panel.SetActive(false);
    }
}