using System;
using System.Collections.Generic;

[Serializable]
public class UnitSaveData
{
    // UnitLevel
    public int Level;
    public long Exp;
    
    // UnitEquipment
    public Dictionary<EquipPart, string> Equipments;
}
