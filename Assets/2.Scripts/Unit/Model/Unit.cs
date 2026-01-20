using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Unit
{
    [SerializeField] private int level;
    public int Level
    {
        get => level;
        set
        {
            level = value;
            OnLevelChanged?.Invoke(level);
        }
    }
    
    [SerializeField] private int hp;
    public int Hp
    {
        get => hp;
        set 
        {
            if (hp == value) return;
            int newValue = Math.Clamp(value, 0, maxHp);
            
            hp = newValue;
            OnHpChanged?.Invoke(maxHp, hp);

            isDeath = Hp <= 0;
        }
    }

    public void TakeDamage(int damage)
    {
        if (damage - def < 0) return;
        Hp -= (damage - def);
    }

    [SerializeField] private int maxHp;
    public int MaxHp
    {
        get => maxHp;
        set => maxHp = value;
    }

    [SerializeField] private int atk;
    public int Atk
    {
        get => atk;
        set
        {
            atk = value;
            OnAtkChanged?.Invoke(atk);
        }
    }

    [SerializeField] private int def;
    public int Def
    {
        get => def;
        set
        {
            def = value;
            OnDefChanged?.Invoke(def);
        }
    }

    [SerializeField] private int speed;
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

    [SerializeField] private bool isDeath;
    public bool IsDeath => isDeath;

    public Action<int> OnLevelChanged;
    public Action<int,int> OnHpChanged;
    public Action<int> OnAtkChanged;
    public Action<int> OnDefChanged;
    public Action<int> OnSpeedChanged;
}
