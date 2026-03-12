using System.Collections.Generic;

public class Party
{
    private List<PartySlot> Slots = new(UnitManager.MaxPartySize);
    
    public bool HasUnit(UnitName unitName)
    {
        foreach (PartySlot partySlot in Slots)
        {
            if (partySlot.UnitName == unitName)
            {
                return true;
            }
        }
        
        return false;
    }
    
    public bool IsSlotEmpty(int slotIdx)
    {
        if (IsInvalidSlotIndex(slotIdx)) return false;
        
        return Slots[slotIdx].UnitName == UnitName.None;
    }

    private static bool IsInvalidSlotIndex(int slotIdx)
    {
        return 0 > slotIdx || slotIdx >= UnitManager.MaxPartySize;
    }

    public int FindUnitSlotIndex(UnitName unitName)
    {
        for (int i = 0; i < Slots.Count; i++)
        {
            if (Slots[i].UnitName == unitName)
            {
                return i;
            }
        }

        return -1;
    }

    public void SetSlot(int slotIdx, UnitName unitName)
    {
        if (IsInvalidSlotIndex(slotIdx)) return;

        Slots[slotIdx].UnitName = unitName;
    }
    
    public void SwapPartySlot(int slotA, int slotB)
    {
        UnitName temp = Slots[slotA].UnitName;
        Slots[slotA].UnitName = Slots[slotB].UnitName;
        Slots[slotB].UnitName = temp;
    }
}
