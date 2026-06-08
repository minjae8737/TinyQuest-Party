using System;
using System.Collections.Generic;

[Serializable]
public class UnitSaveData
{
    // UnitLevel
    public UnitName UnitName;
    public int Level;
    public long Exp;

    public int StarGrade;
    
    // UnitEquipment
    // public Dictionary<EquipPart, string> Equipments;
}
