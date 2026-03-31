// UniversalPopupUI.cs
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Reusable general-purpose popup with up to three buttons.
/// 
/// Caller:
/// Other systems call Show(...) and optionally pass button labels/actions.
/// 
/// Why this is better:
/// - SRP: this class only displays and wires popup UI.
/// - DRY: button setup uses one helper method for all three buttons.
/// - YAGNI: supports only the features the project currently needs.
/// </summary>
public class UniversalPopupUI : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text bodyText;
    [SerializeField] private Image iconImage;

    [Header("Buttons")]
    [SerializeField] private Button button1;
    [SerializeField] private TMP_Text button1Text;

    [SerializeField] private Button button2;
    [SerializeField] private TMP_Text button2Text;

    [SerializeField] private Button button3;
    [SerializeField] private TMP_Text button3Text;

    private void Awake()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
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

        if (titleText != null) titleText.text = title;
        if (bodyText != null) bodyText.text = body;

        ConfigureIcon(icon);

        SetupButton(button1, button1Text, btn1Label, btn1Action);
        SetupButton(button2, button2Text, btn2Label, btn2Action);
        SetupButton(button3, button3Text, btn3Label, btn3Action);
    }

    private void ConfigureIcon(Sprite icon)
    {
        if (iconImage == null) return;

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

    /// <summary>
    /// Shared setup logic for each popup button.
    /// DRY: one method configures all buttons.
    /// </summary>
    private void SetupButton(Button button, TMP_Text label, string text, Action action)
    {
        if (button == null || label == null) return;

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
        {
            panel.SetActive(false);
        }
    }
}