using UnityEngine;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    [Range(0f, 1f)] public float dailyEventChance = 0.35f; // Event probabilities
    [Range(0f, 1f)] public float actionEventChance = 0.25f;

    public List<EventData> eventPool = new List<EventData>(); // Event pool

    [SerializeField] private EventUI eventUI; // UI reference 

    private void Awake()
    {
        if (!eventUI) eventUI = FindObjectOfType<EventUI>();
    }

    public void TryTriggerDailyEvent() // Trigger dailyt events
    {
        if (eventPool.Count == 0) return;
        if (Random.value <= dailyEventChance)
            TriggerRandomEvent();
    }

    public void TryTriggerActionEvent() // Triggering action-based events
    {
        if (eventPool.Count == 0) return;
        if (Random.value <= actionEventChance)
            TriggerRandomEvent();
    }

    private void TriggerRandomEvent()
    {
        var e = eventPool[Random.Range(0, eventPool.Count)];
        eventUI.Show(e);
    }
}
