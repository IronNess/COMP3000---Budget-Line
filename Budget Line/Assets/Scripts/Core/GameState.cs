using UnityEngine;
using System;

public class GameState : MonoBehaviour
{
    [Header("Core Stats")]
    public int money = 0;
    public int energy = 70;
    public int stress = 20;
    public int hunger = 70;
    public int hygiene = 70;
    public int grades = 50;

    [Header("Debt / Interest")]
    public int debtInterestPerDay = 5;

    [Header("Progression Buffs")]
    [Tooltip("Higher = studying costs less energy.")]
    public float studyEfficiency = 1f;

    [Header("Event Flags")]
    public bool rentMissed = false;
    public bool landlordWarningShown = false;
    public bool evictionRiskShown = false;
    public bool burnoutRisk = false;

    [Tooltip("Lower = work/uni causes less stress.")]
    public float resilience = 1f;

    public event Action OnStatsChanged;

    public int GetMoney() => money;
    public int GetEnergy() => energy;
    public int GetStress() => stress;
    public int GetHunger() => hunger;
    public int GetHygiene() => hygiene;
    public int GetGrades() => grades;

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
        grades = Mathf.Clamp(grades + amount, 0, 100);
        OnStatsChanged?.Invoke();
    }

    public void ApplyTimeBlockDecay()
    {
        AddEnergy(-3);
        AddHunger(-2);
        AddHygiene(-1);
    }

    /// <summary>
    /// Runs once per day (when time wraps Night -> Morning)
    /// </summary>
    public void ApplyDailyConsequences()
    {
        ApplyPassiveDailyDecay();
        ApplyDebtInterestIfOverdrawn();
        ApplyHungerCrash();
        ApplyHygieneCrash();
        ApplyBurnoutCrash();
        ApplyLowHungerEnergyPenalty();
        ApplyExhaustionOrStressGradePenalty();
        UpdateBurnoutRiskFlag();
    }

    private void ApplyPassiveDailyDecay()
    {
        AddHunger(-10);
        AddHygiene(-8);
        AddStress(+5);
        AddGrades(-1);
    }

    private void ApplyDebtInterestIfOverdrawn()
    {
        if (money < 0)
        {
            money -= debtInterestPerDay;
            OnStatsChanged?.Invoke();
        }
    }

    private void ApplyHungerCrash()
    {
        if (hunger <= 0)
        {
            AddEnergy(-20);
            AddStress(+15);
        }
    }

    private void ApplyHygieneCrash()
    {
        if (hygiene <= 0)
            AddStress(+10);
    }

    private void ApplyBurnoutCrash()
    {
        if (stress >= 100)
        {
            energy = Mathf.Clamp(energy - 15, 0, 100);
            stress = 70;
            OnStatsChanged?.Invoke();
        }
    }

    private void ApplyLowHungerEnergyPenalty()
    {
        if (hunger < 20)
        {
            energy = Mathf.Clamp(energy - 5, 0, 100);
            OnStatsChanged?.Invoke();
        }
    }

    private void ApplyExhaustionOrStressGradePenalty()
    {
        if (energy <= 10 || stress >= 80)
            AddGrades(-1);
    }

    private void UpdateBurnoutRiskFlag()
    {
        if (stress >= 85)
            burnoutRisk = true;
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
