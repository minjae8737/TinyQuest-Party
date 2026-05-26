using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitSlotUI : ClickSlotUI
{
    [Header("=== Reference ===")]
    public PartySetupPanel partySetupPanel;

    [SerializeField] protected Image unitImage;
    [SerializeField] protected TMP_Text unitNameText;
    [SerializeField] protected TMP_Text unitLevelText;
    [SerializeField] protected StarGradeUI StarGradeUI;
    
    [SerializeField] protected CanvasGroup group;
    [SerializeField] private Button PartySetupButton;
    
    protected UnitSlotDTO dto;
    public UnitName UnitName => dto.UnitName;
    public UnitClass UnitClass => dto.UnitClass;
    
    public void SetSlot(UnitSlotDTO unitSlotDto, Sprite starSprite)
    {
        PartySetupButton?.onClick.RemoveAllListeners();
        PartySetupButton?.onClick.AddListener(OnClickPartySetupButton);
        
        dto = unitSlotDto;
        
        unitImage.sprite = dto.Data.Icon;
        unitNameText.text = dto.Data.UnitName+"";
        unitLevelText.text = $"Lv.{dto.UnitLevel}";

        StarGradeUI.SetStars(dto.StarGrade, starSprite);
    }

    protected override void OnClickCard()
    {
        partySetupPanel.SelectUnitSlot(this);
    }
    
    public virtual void Select()
    {
        UIEffect.FadeIn(group);
    }
    
    public virtual void UnSelect()
    {
        UIEffect.FadeOut(group);
    }

    private void OnClickPartySetupButton()
    {
        partySetupPanel.EnterPartySetupMode();
    }
}
