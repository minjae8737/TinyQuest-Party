using System;
using UnityEngine;

[Serializable]
public class UnitLevel
{
    [SerializeField] private int level;
    [SerializeField] private int exp;
    [SerializeField] private int maxExp;

    public int Level
    {
        get => level;
        set
        {
            level = value;
            OnLevelChanged?.Invoke(level);
        }
    }

    public int Exp
    {
        get => exp;
        set { exp = value; }
    }

    public int MaxExp
    {
        get => maxExp;
        set => maxExp = value;
    }

    public event Action<int> OnLevelChanged;
}