using System;

[Serializable]
public class Stat
{
    public int MaxHp;
    public int Atk;
    public int Def;
    public int Speed;

    public static Stat operator +(Stat a, Stat b)
    {
        return new Stat
        {
            MaxHp = a.MaxHp + b.MaxHp,
            Atk = a.Atk + b.Atk,
            Def = a.Def + b.Def,
            Speed = a.Speed + b.Speed
        };
    }

    public Stat Clone()
    {
        return new Stat
        {
            MaxHp = MaxHp,
            Atk = Atk,
            Def = Def,
            Speed = Speed
        };
    }
}