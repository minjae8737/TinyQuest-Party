using System.Collections.Generic;

public class Party
{
    private List<PartySlot> slots = new();

    public List<PartySlot> Slots => slots;

    public Party()
    {
        for (int i = 0; i < UnitManager.MaxPartySize; i++)
        {
            slots.Add(new PartySlot());
        }
    }

    public bool HasUnit(UnitName unitName)
    {
        foreach (PartySlot partySlot in slots)
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
        
        return slots[slotIdx].UnitName == UnitName.None;
    }

    private bool IsInvalidSlotIndex(int slotIdx)
    {
        return 0 > slotIdx || slotIdx >= UnitManager.MaxPartySize;
    }

    public int FindUnitSlotIndex(UnitName unitName)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].UnitName == unitName)
            {
                return i;
            }
        }

        return -1;
    }

    public void SetSlot(int slotIdx, UnitName unitName)
    {
        if (IsInvalidSlotIndex(slotIdx)) return;

        slots[slotIdx].UnitName = unitName;
    }
    
    public void SwapPartySlot(int slotA, int slotB)
    {
        UnitName temp = slots[slotA].UnitName;
        slots[slotA].UnitName = slots[slotB].UnitName;
        slots[slotB].UnitName = temp;
    }
}
