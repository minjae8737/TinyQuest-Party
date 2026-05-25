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
        }
    }

    public long Exp
    {
        get => exp;
        set { exp = value; }
    }

    public long MaxExp => maxExp;
    public int MaxLevel => ExpCalculator.Instance.GetMaxLevel();

    #endregion
    
    public event Action<int> OnLevelChanged;

    public void Init()
    {
        level = 1;
        exp = 0;
    }

    public bool LevelUp()
    {
        if (level == MaxLevel) return false;
        
        long curExp = CurrencyManager.Instance.Exp;

        if (curExp < MaxExp) return false;

        level++;
        CurrencyManager.Instance.SpendExp(MaxExp);
        OnLevelChanged?.Invoke(level);
        return true;
    }

}