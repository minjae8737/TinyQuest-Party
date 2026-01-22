using System;
using UnityEngine;

[Serializable]
public class UnitStatus
{
    [SerializeField] private int maxHp;
    [SerializeField] private int hp;
    [SerializeField] private bool isDeath;

    public int MaxHp
    {
        get => maxHp;
        set
        {
            if (maxHp == value) return;
            maxHp = value;
            hp = Math.Clamp(hp, 0, maxHp);
        }
    }
    
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

    public bool IsDeath => isDeath;

    public void TakeDamage(int damage)
    {
        if (damage < 0) return;
        Hp -= damage;
    }

    public void Init(int maxHp, int hp)
    {
        MaxHp = maxHp;
        Hp = hp;
    }

    public event Action<int, int> OnHpChanged;
}