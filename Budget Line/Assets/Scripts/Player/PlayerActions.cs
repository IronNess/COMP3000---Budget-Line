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

    // ===== UNIVERSITY =====
    public void GoToUniversity()
    {
        state.AddGrades(+2);
        state.AddEnergy(-10);
        state.AddStress(+3);

        time.AdvanceTime(1);
    }

    // ===== WORK (ORIGINAL) =====
    public void WorkShort()
    {
        state.AddMoney(+30);
        state.AddEnergy(-8);
        state.AddStress(+2);

        time.AdvanceTime(1);
    }

    public void WorkLong()
    {
        state.AddMoney(+70);
        state.AddEnergy(-20);
        state.AddStress(+6);

        time.AdvanceTime(2);
    }

    // ===== WORK (NEW HOURLY OPTIONS) =====
    public void Work2Hours()
    {
        state.AddMoney(+20);
        state.AddEnergy(-5);
        state.AddStress(+1);

        time.AdvanceTime(1);
    }

    public void Work4Hours()
    {
        state.AddMoney(+40);
        state.AddEnergy(-10);
        state.AddStress(+3);

        time.AdvanceTime(2);
    }

    public void Work6Hours()
    {
        state.AddMoney(+65);
        state.AddEnergy(-15);
        state.AddStress(+5);

        time.AdvanceTime(3);
    }

    public void Work8Hours()
    {
        state.AddMoney(+90);
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
        time.AdvanceTime(2);
        state.AddMoney(-5);
        state.AddStress(-3);
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
        state.AddGrades(+1);
        state.AddEnergy(-5);

        time.AdvanceTime(1);
    }

    public void StudyLots()
    {
        state.AddGrades(+3);
        state.AddEnergy(-15);
        state.AddStress(+5);

        time.AdvanceTime(2);
    }

    // ===== WELLBEING / SOCIAL =====
    public void GoToGym()
    {
        state.AddEnergy(-12);
        state.AddStress(-6);
        state.AddHunger(-8);
        state.AddMoney(-10);

        

        time.AdvanceTime(2);
    }

    public void GoDowntown()
    {
        state.AddMoney(-30);
        state.AddEnergy(-8);
        state.AddStress(-8);

        time.AdvanceTime(2);
    }
}