using System.Collections.Generic;

public enum EquipPart
{
    Head,
    Body,
    Feet,
    MainHand,
    OffHand
}

public class UnitEquipment
{
    private Dictionary<EquipPart, long> equipments;
    public Dictionary<EquipPart, long> Equipments => equipments;
    
    public void ApplySaveData(Dictionary<EquipPart, long> saveData)
    {
        equipments = new Dictionary<EquipPart, long>(saveData);
    }
}
