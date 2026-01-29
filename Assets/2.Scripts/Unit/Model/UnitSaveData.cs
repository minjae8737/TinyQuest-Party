using System;
using System.Collections.Generic;

[Serializable]
public class UnitSaveData
{
    // UnitLevel
    public int Level;
    public int Exp;
    public int MaxExp;
    
    // UnitEquipment
    public Dictionary<EquipPart, long> Equipments;
}
