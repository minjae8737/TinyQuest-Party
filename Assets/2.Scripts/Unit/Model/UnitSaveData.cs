using System;
using System.Collections.Generic;

[Serializable]
public class UnitSaveData
{
    public string UnitName;
    
    // UnitLevel
    public int Level;
    public int Exp;
    public int MaxExp;
    
    // UnitStat
    public Stat BaseStat;
    public Stat EquipStat;
    
    // UnitEquipment
    public Dictionary<EquipPart, long> Equipments;
}
