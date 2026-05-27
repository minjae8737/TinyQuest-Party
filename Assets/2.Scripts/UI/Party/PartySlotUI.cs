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
        if (Blank.activeSelf) return;
        
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
        bool result = UnitManager.Instance.AssignUnitToSlot(slotIdx, UnitName.None);
        if (!result)
        {
            string title = "알림";
            string message = "파티에서 나갈 수 없습니다.";
            string confirm = "확인";
            PopupManager.Instance.ShowConfirmPopup(title, message, confirm);
        }
    }
}
