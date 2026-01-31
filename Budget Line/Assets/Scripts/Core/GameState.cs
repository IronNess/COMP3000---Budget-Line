using UnityEngine;
using System;

public class GameState : MonoBehaviour
{
    [Header("Core Stats")]
    public int money = 200;
    public int energy = 70;   // 0..100
    public int stress = 20;   // 0..100
    public int hunger = 70;   // 0..100
    public int hygiene = 70;  // 0..100
    public int grades = 0;

    [Header("Debt / Interest")]
    public int debtInterestPerDay = 5;

    [Header("Progression Buffs")]
    [Tooltip("Higher = studying costs less energy.")]
    public float studyEfficiency = 1f; // 1..1.5
    [Tooltip("Lower = work/uni causes less stress.")]
    public float resilience = 1f;      // 0.7..1

    public event Action OnStatsChanged;

    public void AddMoney(int amount)
    {
        money += amount;
        OnStatsChanged?.Invoke();
    }

    public void AddEnergy(int amount)
    {
        energy = Mathf.Clamp(energy + amount, 0, 100);
        OnStatsChanged?.Invoke();
    }

    public void AddStress(int amount)
    {
        stress = Mathf.Clamp(stress + amount, 0, 100);
        OnStatsChanged?.Invoke();
    }

    public void AddHunger(int amount)
    {
        hunger = Mathf.Clamp(hunger + amount, 0, 100);
        OnStatsChanged?.Invoke();
    }

    public void AddHygiene(int amount)
    {
        hygiene = Mathf.Clamp(hygiene + amount, 0, 100);
        OnStatsChanged?.Invoke();
    }

    public void AddGrades(int amount)
    {
        grades += amount;
        OnStatsChanged?.Invoke();
    }

    /// <summary>
    /// Runs once per day (when time wraps Night->Morning).
    /// Handles passive decay + soft fail states.
    /// </summary>
    public void ApplyDailyConsequences()
    {
        // Passive decay / pressure
        AddHunger(-10);
        AddHygiene(-8);
        AddStress(+5);

        // Debt snowball
        if (money < 0)
        {
            money -= debtInterestPerDay;
        }

        // Hunger crash (soft fail)
        if (hunger <= 0)
        {
            AddEnergy(-20);
            AddStress(+15);
        }

        // Hygiene crash (soft fail)
        if (hygiene <= 0)
        {
            AddStress(+10);
        }

        // Stress critical → burnout (soft fail)
        if (stress >= 100)
        {
            // forced "rest" effect: you lose energy and still remain stressed
            energy = Mathf.Clamp(energy - 20, 0, 100);
            stress = 70;
            OnStatsChanged?.Invoke();
        }

        if (hunger < 20)
        {
            energy = Mathf.Clamp(energy - 5, 0, 100);
            OnStatsChanged?.Invoke();
        }

    }

    public void ImproveStudyEfficiency()
    {
        studyEfficiency = Mathf.Clamp(studyEfficiency + 0.05f, 1f, 1.5f);
        OnStatsChanged?.Invoke();
    }

    public void ImproveResilience()
    {
        resilience = Mathf.Clamp(resilience - 0.03f, 0.7f, 1f);
        OnStatsChanged?.Invoke();
    }

    public bool laptopBroken = false;
}
