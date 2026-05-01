using System;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private long maxHp;
    [SerializeField] private long atk;
    [SerializeField] private long def;
    [SerializeField] private long speed;

    #region Property
    
    public long MaxHp
    {
        get => maxHp;
        set => maxHp = Math.Max(0, value);
    }

    public long Atk
    {
        get => atk;
        set => atk = Math.Max(0, value);
    }

    public long Def
    {
        get => def;
        set => def = Math.Max(0, value);
    }

    public long Speed
    {
        get => speed;
        set => speed = Math.Max(0, value);
    }

    #endregion
    
    public static Stat operator +(Stat a, Stat b)
    {
        return new Stat
        {
            maxHp = a.MaxHp + b.MaxHp,
            atk = a.Atk + b.Atk,
            def = a.Def + b.Def,
            speed = a.Speed + b.Speed
        };
    }    

    public static Stat operator -(Stat a, Stat b)
    {
        return new Stat
        {
            maxHp = a.MaxHp - b.MaxHp,
            atk = a.Atk - b.Atk,
            def = a.Def - b.Def,
            speed = a.Speed - b.Speed
        };
    }

    public static Stat operator *(Stat a, float b)
    {
        return new Stat
        {
            maxHp = (long)(a.MaxHp * b),
            atk = (long)(a.Atk * b),
            def = (long)(a.Def * b),
            speed = a.Speed
        };
    }

    public void Add(Stat a)
    {
        maxHp += a.MaxHp;
        atk += a.Atk;
        def += a.Def;
        speed += a.Speed;
    }

    public void Subtrack(Stat a)
    {
        maxHp -= a.MaxHp;
        atk -= a.Atk;
        def -= a.Def;
        speed -= a.Speed;
    }

    public Stat Clone()
    {
        return new Stat
        {
            maxHp = MaxHp,
            atk = Atk,
            def = Def,
            speed = Speed
        };
    }
}