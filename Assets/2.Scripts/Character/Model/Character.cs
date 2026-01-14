using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Character
{
    private int level;
    public int Level
    {
        get => level;
        set
        {
            level = value;
            OnLevelChanged?.Invoke(level);
        }
    }
    
    private int hp;
    public int Hp
    {
        get => hp;
        set 
        {
            if (hp == value) return;
            int newValue = Math.Clamp(value, 0, maxHp);
            
            hp = newValue;
            OnHpChanged?.Invoke(hp);
        }
    }

    public void TakeDamage(int damage)
    {
        if (damage - def < 0) return;
        Hp -= (damage - def);
    }

    private int maxHp;
    public int MaxHp
    {
        get => maxHp;
        set => maxHp = value;
    }

    private int atk;
    public int Atk
    {
        get => atk;
        set
        {
            atk = value;
            OnAtkChanged?.Invoke(atk);
        }
    }

    private int def;
    public int Def
    {
        get => def;
        set
        {
            def = value;
            OnDefChanged?.Invoke(def);
        }
    }

    private int speed;
    public int Speed
    {
        get => speed;
        set
        {
            speed = value;
            OnSpeedChanged?.Invoke(speed);
        }
    }

    public List<Skill> Skills;

    public Skill GetSkill(int skillIdx)
    {
        if (skillIdx >= Skills.Count) return null;   
        return Skills[skillIdx];
    }

    public Action<int> OnLevelChanged;
    public Action<int> OnHpChanged;
    public Action<int> OnAtkChanged;
    public Action<int> OnDefChanged;
    public Action<int> OnSpeedChanged;
}
