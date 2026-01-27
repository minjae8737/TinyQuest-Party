using System;

[Serializable]
public class Stat
{
    private int maxHp;
    private int atk;
    private int def;
    private int speed;

    public int MaxHp
    {
        get => maxHp;
        set => maxHp = Math.Max(0, value);
    }

    public int Atk
    {
        get => atk;
        set => atk = Math.Max(0, value);
    }

    public int Def
    {
        get => def;
        set => def = Math.Max(0, value);
    }

    public int Speed
    {
        get => speed;
        set => speed = Math.Max(0, value);
    }

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