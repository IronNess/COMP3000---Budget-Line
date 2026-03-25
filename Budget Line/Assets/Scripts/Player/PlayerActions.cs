using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private GameState state;
    [SerializeField] private TimeSystem time;

    private void Awake()
    {
        if (!state) state = FindObjectOfType<GameState>();
        if (!time) time = FindObjectOfType<TimeSystem>();
    }

    private void ApplyStatPenaltiesBeforeAction()
    {
        // Hunger too low = weaker and more stressed
        if (state.hunger < 20)
        {
            state.AddEnergy(-5);
            state.AddStress(+5);
        }

        // Hygiene too low = extra stress
        if (state.hygiene < 20)
        {
            state.AddStress(+5);
        }

        // Stress too high = extra penalty
        if (state.stress > 70)
        {
            state.AddStress(+2);
            state.AddEnergy(-2);
        }
    }

    // ===== UNIVERSITY =====
    public void GoToUniversity()
    {
        ApplyStatPenaltiesBeforeAction();

        int gradeGain = state.energy < 20 ? 0 : 2;

        state.AddGrades(gradeGain);
        state.AddEnergy(-10);
        state.AddStress(+3);

        time.AdvanceTime(1);
    }

    // ===== WORK (ORIGINAL) =====
    public void WorkShort()
    {
        ApplyStatPenaltiesBeforeAction();

        int moneyGain = state.energy < 20 ? 20 : 30;

        state.AddMoney(moneyGain);
        state.AddEnergy(-8);
        state.AddStress(+2);

        time.AdvanceTime(1);
    }

    public void WorkLong()
    {
        ApplyStatPenaltiesBeforeAction();

        int moneyGain = state.energy < 20 ? 50 : 70;

        state.AddMoney(moneyGain);
        state.AddEnergy(-20);
        state.AddStress(+6);

        time.AdvanceTime(2);
    }

    // ===== WORK (NEW HOURLY OPTIONS) =====
    public void Work2Hours()
    {
        ApplyStatPenaltiesBeforeAction();

        int moneyGain = state.energy < 20 ? 15 : 20;

        state.AddMoney(moneyGain);
        state.AddEnergy(-5);
        state.AddStress(+1);

        time.AdvanceTime(1);
    }

    public void Work4Hours()
    {
        ApplyStatPenaltiesBeforeAction();

        int moneyGain = state.energy < 20 ? 30 : 40;

        state.AddMoney(moneyGain);
        state.AddEnergy(-10);
        state.AddStress(+3);

        time.AdvanceTime(2);
    }

    public void Work6Hours()
    {
        ApplyStatPenaltiesBeforeAction();

        int moneyGain = state.energy < 20 ? 45 : 65;

        state.AddMoney(moneyGain);
        state.AddEnergy(-15);
        state.AddStress(+5);

        time.AdvanceTime(3);
    }

    public void Work8Hours()
    {
        ApplyStatPenaltiesBeforeAction();

        int moneyGain = state.energy < 20 ? 60 : 90;

        state.AddMoney(moneyGain);
        state.AddEnergy(-25);
        state.AddStress(+8);

        time.AdvanceTime(4);
    }

    // ===== FOOD =====
    public void EatQuickMeal()
    {
        state.AddHunger(+10);
        state.AddMoney(-5);

        time.AdvanceTime(1);
    }

    public void EatBigMeal()
    {
        state.AddHunger(+30);
        state.AddMoney(-15);

        time.AdvanceTime(2);
    }

    // ===== HYGIENE =====
    public void Shower()
    {
        state.AddHygiene(+10);
        time.AdvanceTime(1);
    }

    public void Bath()
    {
        state.AddHygiene(+30);
        state.AddMoney(-5);
        state.AddStress(-3);

        time.AdvanceTime(2);
    }

    public void DoLaundry()
    {
        state.AddHygiene(+15);
        state.AddEnergy(-5);
        state.AddStress(+1);
        state.AddMoney(-20);

        time.AdvanceTime(1);
    }

    // ===== SLEEP =====
    public void Nap()
    {
        state.AddEnergy(+15);
        state.AddStress(-5);

        time.AdvanceTime(1);
    }

    public void FullSleep()
    {
        state.AddEnergy(+40);
        state.AddStress(-10);
        state.AddHunger(-10);
        state.AddHygiene(-5);

        time.AdvanceTime(3);
    }

    // ===== STUDY =====
    public void StudyLittle()
    {
        ApplyStatPenaltiesBeforeAction();

        int gradeGain = state.energy < 20 ? 0 : 1;

        state.AddGrades(gradeGain);
        state.AddEnergy(-5);

        time.AdvanceTime(1);
    }

    public void StudyLots()
    {
        ApplyStatPenaltiesBeforeAction();

        int gradeGain = state.energy < 20 ? 1 : 3;

        state.AddGrades(gradeGain);
        state.AddEnergy(-15);
        state.AddStress(+5);

        time.AdvanceTime(2);
    }

    // ===== WELLBEING / SOCIAL =====
    public void GoToGym()
    {
        ApplyStatPenaltiesBeforeAction();

        state.AddEnergy(-12);
        state.AddStress(-6);
        state.AddHunger(-8);
        state.AddMoney(-10);

        time.AdvanceTime(2);
    }

    public void GoDowntown()
    {
        ApplyStatPenaltiesBeforeAction();

        state.AddMoney(-30);
        state.AddEnergy(-8);
        state.AddStress(-8);

        time.AdvanceTime(2);
    }

    public void WorkBasic()
    {
        ApplyStatPenaltiesBeforeAction();

        int moneyGain = state.energy < 20 ? 18 : 25;

        state.AddMoney(moneyGain);
        state.AddEnergy(-8);
        state.AddStress(+4);
        time.AdvanceTime(1);
    }

    public void WorkCampusJob()
    {
        if (state.grades < 3)
        {
            Debug.Log("Not enough grades for this job.");
            return;
        }

        ApplyStatPenaltiesBeforeAction();

        int moneyGain = state.energy < 20 ? 30 : 45;

        state.AddMoney(moneyGain);
        state.AddEnergy(-10);
        state.AddStress(+5);
        time.AdvanceTime(1);
    }

    public void WorkHighSkill()
    {
        if (state.grades < 6)
        {
            Debug.Log("Not enough grades for this job.");
            return;
        }

        ApplyStatPenaltiesBeforeAction();

        int moneyGain = state.energy < 20 ? 50 : 70;

        state.AddMoney(moneyGain);
        state.AddEnergy(-14);
        state.AddStress(+6);
        time.AdvanceTime(1);
    }
}