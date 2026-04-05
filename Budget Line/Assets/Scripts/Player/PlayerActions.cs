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

    private bool IsLowEnergy()
    {
        return state.energy < 20;
    }

    private int MoneyGainTiredVersusRested(int ifTired, int ifRested)
    {
        return IsLowEnergy() ? ifTired : ifRested;
    }

    private void ApplyStatPenaltiesBeforeAction()
    {
        if (state.hunger < 20)
        {
            state.AddEnergy(-5);
            state.AddStress(+5);
        }

        if (state.hygiene < 20)
            state.AddStress(+5);

        if (state.stress > 70)
        {
            state.AddStress(+2);
            state.AddEnergy(-2);
        }
    }

    private void DoShiftWork(int moneyIfTired, int moneyIfRested, int energyCost, int stressDelta, int timeBlocks)
    {
        ApplyStatPenaltiesBeforeAction();
        state.AddMoney(MoneyGainTiredVersusRested(moneyIfTired, moneyIfRested));
        state.AddEnergy(-energyCost);
        state.AddStress(stressDelta);
        time.AdvanceTime(timeBlocks);
    }

    public void GoToUniversity()
    {
        ApplyStatPenaltiesBeforeAction();

        int gradeGain = IsLowEnergy() ? 0 : 2;

        state.AddGrades(gradeGain);
        state.AddEnergy(-10);
        state.AddStress(+3);

        time.AdvanceTime(1);
    }

    public void WorkShort() => DoShiftWork(20, 30, 8, +2, 1);

    public void WorkLong() => DoShiftWork(50, 70, 20, +6, 2);

    public void Work2Hours() => DoShiftWork(15, 20, 5, +1, 1);

    public void Work4Hours() => DoShiftWork(30, 40, 10, +3, 2);

    public void Work6Hours() => DoShiftWork(45, 65, 15, +5, 3);

    public void Work8Hours() => DoShiftWork(60, 90, 25, +8, 4);

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

    public void StudyLittle()
    {
        ApplyStatPenaltiesBeforeAction();

        int gradeGain = IsLowEnergy() ? 0 : 1;

        state.AddGrades(gradeGain);
        state.AddEnergy(-5);

        time.AdvanceTime(1);
    }

    public void StudyLots()
    {
        ApplyStatPenaltiesBeforeAction();

        int gradeGain = IsLowEnergy() ? 1 : 3;

        state.AddGrades(gradeGain);
        state.AddEnergy(-15);
        state.AddStress(+5);

        time.AdvanceTime(2);
    }

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

    public void WorkBasic() => DoShiftWork(18, 25, 8, +4, 1);

    public void WorkCampusJob()
    {
        if (state.grades < 3)
        {
            Debug.Log("Not enough grades for this job.");
            return;
        }

        DoShiftWork(30, 45, 10, +5, 1);
    }

    public void WorkHighSkill()
    {
        if (state.grades < 6)
        {
            Debug.Log("Not enough grades for this job.");
            return;
        }

        DoShiftWork(50, 70, 14, +6, 1);
    }
}
