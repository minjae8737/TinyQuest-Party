using System;
using UnityEngine;

[Serializable]
public class UnitStatus
{
    [SerializeField, ReadOnly] private long maxHp;
    [SerializeField, ReadOnly] private long hp;
    [SerializeField, ReadOnly] private bool isDeath;

    #region Property

    public long MaxHp
    {
        get => maxHp;
        set
        {
            if (maxHp == value) return;
            maxHp = value;
            hp = Math.Clamp(hp, 0, maxHp);
        }
    }
    
    public long Hp
    {
        get => hp;
        set
        {
            if (hp == value) return;
            long newValue = Math.Clamp(value, 0, maxHp);

            hp = newValue;
            OnHpChanged?.Invoke(maxHp, hp);

            isDeath = Hp <= 0;
        }
    }

    public bool IsDeath => isDeath;
    
    #endregion
    
    public event Action<long, long> OnHpChanged;

    public void Init(long maxHp, long hp)
    {
        MaxHp = maxHp;
        Hp = hp;
    }

    public void TakeDamage(long damage)
    {
        if (damage < 0) return;
        Hp -= damage;
    }

    public void TakeHeal(long healAmount)
    {
        Hp += healAmount;
    }
}