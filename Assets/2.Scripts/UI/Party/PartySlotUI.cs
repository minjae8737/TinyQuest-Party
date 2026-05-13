using UnityEngine;
using UnityEngine.EventSystems;

public class PartySlotUI : UnitSlotUI, IDropHandler
{
    [SerializeField] private GameObject Blank;

    private int slotIdx;
    
    public void SetSlot(UnitSlotDTO unitSlotDto, Sprite starSprite, int idx)
    {
        slotIdx = idx;
        
        Blank.SetActive(!unitSlotDto.HasUnit);
        if (!unitSlotDto.HasUnit) return;
        
        base.SetSlot(unitSlotDto, starSprite);
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Debug.Log("OnDrop");

        UnitDragContext dragContext = (UnitDragContext)UIManager.Instance.DragContext;
        
        if (dragContext.source is UnitSlotUI)
        {
            UnitManager.Instance.AssignUnitToSlot(slotIdx, dragContext.UnitName);
        }
    }
}
