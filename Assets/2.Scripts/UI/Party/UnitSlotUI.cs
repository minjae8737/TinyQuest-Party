using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitSlotUI : DragSlotUI
{
    [Header("=== Reference ===")]
    [SerializeField] protected Image unitImage;
    [SerializeField] protected TMP_Text unitNameText;
    [SerializeField] protected TMP_Text unitLevelText;
    [SerializeField] protected StarGradeUI StarGradeUI;
    
    protected UnitSlotDTO dto;
    public UnitName UnitName => dto.UnitName;
    public UnitClass UnitClass => dto.UnitClass;
    
    public virtual void SetSlot(UnitSlotDTO unitSlotDto, Sprite starSprite)
    {
        dto = unitSlotDto;
        
        unitImage.sprite = dto.Data.Icon;
        unitNameText.text = dto.Data.UnitName+"";
        unitLevelText.text = $"Lv.{dto.UnitLevel}";

        StarGradeUI.SetStars(dto.StarGrade, starSprite);
    }
    
    #region DragEvent
    
    protected override Image GetDragImage()
    {
        return unitImage;
    }

    protected override bool CanDrag()
    {
        return dto?.HasUnit ?? false;
    }
    
    #endregion
    
    public override void SetDragContext()
    {
        UnitDragContext dragContext = new();
        dragContext.source = this;
        dragContext.UnitName = dto.Data.UnitName;
        
        UIManager.Instance.DragContext = dragContext;
    }

}
