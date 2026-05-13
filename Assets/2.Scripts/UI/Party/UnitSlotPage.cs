using System.Collections.Generic;
using UnityEngine;

public class UnitSlotPage : MonoBehaviour
{
    private List<UnitSlotUI> unitSlotUis = new();
    private const int PageSize = 10;

    public bool IsFull => unitSlotUis.Count >= PageSize;
    public bool IsEmpty => unitSlotUis.Count == 0;

    public bool Add(UnitSlotUI unitSlot)
    {
        unitSlotUis.Add(unitSlot);
        unitSlot.transform.parent = transform;
        
        return true;
    }
    
    public void Clear()
    {
        unitSlotUis.Clear();
    }
}
