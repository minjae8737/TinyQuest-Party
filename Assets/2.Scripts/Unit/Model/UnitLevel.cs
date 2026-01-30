using System;
using UnityEngine;

[Serializable]
public class UnitLevel
{
    [SerializeField] private int level;
    [SerializeField] private long exp;
    [SerializeField] private long maxExp => ExpCalculator.Instance.GetMaxExp(level);

    public int Level
    {
        get => level;
        set
        {
            level = value;
            // OnLevelChanged?.Invoke(level);
        }
    }

    public long Exp
    {
        get => exp;
        set { exp = value; }
    }

    public long MaxExp => maxExp;

    public void Init()
    {
        level = 1;
        exp = 0;
    }

    public void AddExp(long gainedExp)
    {
        long remainedExp = 0L;
        long calMaxExp = maxExp;
        exp += gainedExp;

        if (exp >= calMaxExp)
        {
            remainedExp = exp - calMaxExp;
            exp = remainedExp;
            level++;
            OnLevelChanged?.Invoke(level);
        }
    }

    public event Action<int> OnLevelChanged;
}