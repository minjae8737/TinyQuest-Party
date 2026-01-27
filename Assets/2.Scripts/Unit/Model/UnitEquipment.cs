using System.Collections.Generic;
using UnityEngine;

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
    public IReadOnlyDictionary<EquipPart, long> Equipments => equipments;

    public long GetEquipmentId(EquipPart part)
    {
        return equipments.TryGetValue(part, out var id) ? id : -1L;
    }

    public void SetEquipment(EquipPart part, long itemId)
    {
        equipments[part] = itemId;
    }

    public void RemoveEquipment(EquipPart part)
    {
        equipments.Remove(part);
    }
}
