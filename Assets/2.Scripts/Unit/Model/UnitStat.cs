using System;
using UnityEngine;

[Serializable]
public class UnitStat
{
    public Stat BaseStat;
    public Stat EquipStat;
    public Stat TotalStat;

    public int MaxHp => TotalStat.MaxHp;
    public int Atk => TotalStat.Atk;
    public int Def => TotalStat.Def;
    public int Speed => TotalStat.Speed;
    
    public event Action<int> OnAtkChanged;
    public event Action<int> OnDefChanged;
    public event Action<int> OnSpeedChanged;

    public void RefreshStat()
    {
        TotalStat = BaseStat + EquipStat;
    }
}
