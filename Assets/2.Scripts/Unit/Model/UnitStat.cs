using System;

[Serializable]
public class UnitStat
{
    public Stat BaseStat;
    public Stat EquipStat;
    public Stat TotalStat;

    #region Property
    
    public int MaxHp => TotalStat.MaxHp;
    public int Atk => TotalStat.Atk;
    public int Def => TotalStat.Def;
    public int Speed => TotalStat.Speed;
    
    #endregion

    #region Event
    
    public event Action<int> OnAtkChanged;
    public event Action<int> OnDefChanged;
    public event Action<int> OnSpeedChanged;
    
    #endregion

    public void SetBaseStat(Stat stat)
    {
        BaseStat = stat;
        RefreshStat();
    }
    
    public void SetEquipStat(Stat stat)
    {
        EquipStat = stat;
        RefreshStat();
    }
    
    public void RefreshStat()
    {
        TotalStat = BaseStat + EquipStat;
    }
}
