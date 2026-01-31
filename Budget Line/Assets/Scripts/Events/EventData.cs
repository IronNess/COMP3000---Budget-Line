using UnityEngine;
using System;

[CreateAssetMenu(menuName = "BudgetLine/Event")]
public class EventData : ScriptableObject
{
    public string title;

    [TextArea(3, 8)]
    public string description;

    public EventChoice[] choices;

    [Serializable]
    public class EventChoice
    {
        public string label;

        public int moneyDelta;
        public int energyDelta;
        public int stressDelta;
        public int hungerDelta;
        public int hygieneDelta;
        public int gradesDelta;

        public int timeBlocksCost;
    }
}
