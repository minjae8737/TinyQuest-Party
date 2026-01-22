using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Unit
{
    public UnitLevel Level;
    public UnitStat Stat;
    public UnitEquipment Equipment;
    public UnitStatus Status;
    public bool IsDeath => Status.IsDeath;
    
    public List<Skill> Skills;

    public void Init()
    {
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
    
}