using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages random events triggered daily or after player actions.
/// 
/// Caller:
/// - TimeSystem calls TryTriggerDailyEvent()
/// - Other gameplay actions can call TryTriggerActionEvent()
/// 
/// Why this is better:
/// - SRP: this class only decides whether and which event should trigger.
/// - DRY: both public trigger methods reuse shared internal logic.
/// - YAGNI: keeps a simple random pool rather than a large weighted event system.
/// </summary>
public class EventManager : MonoBehaviour
{
    [Header("Event Chances")]
    [Range(0f, 1f)] [SerializeField] private float dailyEventChance = 0.35f;
    [Range(0f, 1f)] [SerializeField] private float actionEventChance = 0.25f;

    [Header("Event Pool")]
    [SerializeField] private List<EventData> eventPool = new List<EventData>();

    [Header("UI")]
    [SerializeField] private EventUI eventUI;

    private void Awake()
    {
        if (eventUI == null)
        {
            eventUI = FindObjectOfType<EventUI>();
        }
    }

    public void TryTriggerDailyEvent()
    {
        TryTriggerEvent(dailyEventChance);
    }

    public void TryTriggerActionEvent()
    {
        TryTriggerEvent(actionEventChance);
    }

    /// <summary>
    /// Shared event trigger logic.
    /// DRY: both daily and action-based events reuse this method.
    /// </summary>
    private void TryTriggerEvent(float chance)
    {
        if (eventPool == null || eventPool.Count == 0) return;
        if (eventUI == null) return;

        if (Random.value <= chance)
        {
            TriggerRandomEvent();
        }
    }

    private void TriggerRandomEvent()
    {
        EventData chosenEvent = eventPool[Random.Range(0, eventPool.Count)];
        eventUI.Show(chosenEvent);
    }
}