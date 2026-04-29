using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartySlotUI : DragSlotUI, IDropHandler
{
    [Header("=== Reference ===")]
    [SerializeField] private Image unitImage;
    [SerializeField] private Text unitNameText;

    private UnitName unitName;
    private int slotIdx;
    
    public void SetSlot(Sprite unitSptrite, UnitName unitName, int idx)
    {
        unitImage.sprite = unitSptrite;
        unitNameText.text = unitName+"";
        this.unitName = unitName;
        slotIdx = idx;
    }

    public UnitName GetUnitName()
    {
        return unitName;
    }

    #region DragEvent

    protected override Image GetDragImage()
    {
        return unitImage;
    }

    protected override bool CanDrag()
    {
        return unitName != UnitName.None;
    }
    
    #endregion

    public override void SetDragContext()
    {
        UnitDragContext dragContext = new UnitDragContext();
        dragContext.source = this;
        dragContext.UnitName = unitName;
        
        UIManager.Instance.DragContext = dragContext;
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Debug.Log("OnDrop");

        UnitDragContext dragContext = (UnitDragContext)UIManager.Instance.DragContext;

        if (dragContext.source is PartySlotUI || dragContext.source is UnitListSlotUI)
        {
            UnitManager.Instance.AssignUnitToSlot(slotIdx, dragContext.UnitName);
        }
    }
}
