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
    private Dictionary<EquipPart, string> equipments;
    public IReadOnlyDictionary<EquipPart, string> Equipments => equipments;

    public string GetEquipmentId(EquipPart part)
    {
        return equipments.TryGetValue(part, out var id) ? id : "EmptyId";
    }

    public void SetEquipment(EquipPart part, string itemId)
    {
        equipments[part] = itemId;
    }

    public void RemoveEquipment(EquipPart part)
    {
        equipments.Remove(part);
    }
}
