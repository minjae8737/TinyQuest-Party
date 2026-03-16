using UnityEngine;
using UnityEngine.UI;

public class UnitListSlotUI : DragSlotUI
{
    [SerializeField] private Image unitImage;
    [SerializeField] private Text unitNameText;

    private UnitName unitName;
    
    public void SetSlot(Sprite unitSptrite, UnitName unitName)
    {
        unitImage.sprite = unitSptrite;
        unitNameText.text = unitName+"";
        this.unitName = unitName;
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
    
}
