using System;
using UnityEngine;

[Serializable]
public class UnitLevel
{
    [SerializeField, ReadOnly] private int level;
    [SerializeField, ReadOnly] private long exp;
    private long maxExp => ExpCalculator.Instance.GetMaxExp(level);

    #region Property

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
    
    #endregion

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