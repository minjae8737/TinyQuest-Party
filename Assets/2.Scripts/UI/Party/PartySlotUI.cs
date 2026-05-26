using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartySlotUI : UnitCardUI
{
    public PartySetupPanel partySetupPanel;

    [SerializeField] private GameObject Blank;
    [SerializeField] protected CanvasGroup group;
    [SerializeField] private Button LeavePartyButton;

    private int slotIdx;
    
    public void SetSlot(UnitSlotDTO unitSlotDto, Sprite starSprite, int idx)
    {
        LeavePartyButton?.onClick.RemoveAllListeners();
        LeavePartyButton?.onClick.AddListener(OnClickLeavePartyButton);
        
        slotIdx = idx;
        dto = unitSlotDto;
        
        Blank.SetActive(!unitSlotDto.HasUnit);
        if (!unitSlotDto.HasUnit) return;
        
        unitImage.sprite = dto.Data.Icon;
        unitNameText.text = dto.Data.UnitName+"";
        unitLevelText.text = $"Lv.{dto.UnitLevel}";
        
        StarGradeUI.SetStars(dto.StarGrade, starSprite);
    }

    protected override void OnClickCard()
    {
        partySetupPanel.SelectPartySlot(this);
    }
    
    public void Select()
    {
        UIEffect.FadeIn(group);
    }
    
    public void UnSelect()
    {
        UIEffect.FadeOut(group);
    }

    public void ReplaceUnit(UnitName unitName)
    {
        if (UnitName == unitName) return;
        
        UnitManager.Instance.AssignUnitToSlot(slotIdx, unitName);
    }
    
    private void OnClickLeavePartyButton()
    {
        UnitManager.Instance.AssignUnitToSlot(slotIdx, UnitName.None);
    }
}
