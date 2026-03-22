using UnityEngine;
using System;

public class GameState : MonoBehaviour
{
    [Header("Core Stats")]
    public int money = 0; // Starting money is 0 before student loan added
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

    // -----------------------------
    // GETTERS (for UI scripts)
    // -----------------------------

    public int GetMoney()
    {
        return money;
    }

    public int GetEnergy()
    {
        return energy;
    }

    public int GetStress()
    {
        return stress;
    }

    public int GetHunger()
    {
        return hunger;
    }

    public int GetHygiene()
    {
        return hygiene;
    }

    public int GetGrades()
    {
        return grades;
    }

    // -----------------------------
    // STAT MODIFIERS
    // -----------------------------

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

    public void ApplyTimeBlockDecay()
{
    AddEnergy(-5);
    AddHunger(-4);
    AddHygiene(-3);
}

    /// <summary>
    /// Runs once per day (when time wraps Night -> Morning)
    /// </summary>
    public void ApplyDailyConsequences()
    {
        // Passive decay
        AddHunger(-10);
        AddHygiene(-8);
        AddStress(+5);

        // Debt snowball
        if (money < 0)
        {
            money -= debtInterestPerDay;
            OnStatsChanged?.Invoke();
        }

        // Hunger crash
        if (hunger <= 0)
        {
            AddEnergy(-20);
            AddStress(+15);
        }

        // Hygiene crash
        if (hygiene <= 0)
        {
            AddStress(+10);
        }

        // Burnout
        if (stress >= 100)
        {
            energy = Mathf.Clamp(energy - 20, 0, 100);
            stress = 70;
            OnStatsChanged?.Invoke();
        }

        // Low hunger penalty
        if (hunger < 20)
        {
            energy = Mathf.Clamp(energy - 5, 0, 100);
            OnStatsChanged?.Invoke();
        }
    }

    // -----------------------------
    // PROGRESSION
    // -----------------------------

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

    // -----------------------------
    // WORLD FLAGS
    // -----------------------------

    public bool laptopBroken = false;
}