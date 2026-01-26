using System;
using System.Collections.Generic;

[Serializable]
public class Unit
{
    public UnitLevel Level;
    public UnitStat Stat;
    public UnitEquipment Equipment;
    public UnitStatus Status;
    public bool IsDeath => Status.IsDeath;
    
    public List<Skill> Skills;

    public void Init(UnitSaveData saveData)
    {
        // Load SaveData
        // UnitLevel
        Level.Level = saveData.Level;
        Level.Exp = saveData.Exp;
        Level.MaxExp = saveData.MaxExp;
        // Stat
        Stat.BaseStat = saveData.BaseStat.Clone(); 
        Stat.EquipStat = saveData.EquipStat.Clone(); 
        // Equipment
        Equipment.ApplySaveData(saveData.Equipments);
        
        Stat.RefreshStat();
        Status.Init(Stat.MaxHp, Stat.MaxHp);
    }

    public void TakeDamage(int damage)
    {
        if (damage - Stat.Def < 0) return;
        damage -= Stat.Def;
        Status.TakeDamage(damage);
    }

    public Skill GetSkill(int skillIdx)
    {
        if (skillIdx >= Skills.Count) return null;
        return Skills[skillIdx];
    }

    public event Action<int> OnLevelChanged
    {
        add => Level.OnLevelChanged += value;
        remove => Level.OnLevelChanged -= value;
    }
    
    public event Action<int, int> OnHpChanged
    {
        add => Status.OnHpChanged += value;
        remove => Status.OnHpChanged -= value;
    }
    
    public event Action<int> OnAtkChanged
    {
        add => Stat.OnAtkChanged += value;
        remove => Stat.OnAtkChanged -= value;
    }
    
    public event Action<int> OnDefChanged
    {
        add => Stat.OnDefChanged += value;
        remove => Stat.OnDefChanged -= value;
    }
    
    public event Action<int> OnSpeedChanged
    {
        add => Stat.OnSpeedChanged += value;
        remove => Stat.OnSpeedChanged -= value;
    }

    public UnitSaveData GetSaveData()
    {
        UnitSaveData saveData = new UnitSaveData();
        
        saveData.UnitName = "";
        
        saveData.Level = Level.Level;
        saveData.Exp = Level.Exp;
        saveData.MaxExp = Level.MaxExp;
        
        saveData.BaseStat = Stat.BaseStat.Clone();
        saveData.EquipStat = Stat.EquipStat.Clone();

        saveData.Equipments = new Dictionary<EquipPart, long>(Equipment.Equipments);
        
        return saveData;
    }
}