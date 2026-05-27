using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyCardUI : UnitCardUI
{
    public PartySetupPanel partySetupPanel;
    
    [SerializeField] protected CanvasGroup group;
    [SerializeField] private Button PartySetupButton;
    
    public override void SetSlot(UnitSlotDTO unitSlotDto, Sprite starSprite)
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
    
    public void Select()
    {
        UIEffect.FadeIn(group);
    }
    
    public void UnSelect()
    {
        UIEffect.FadeOut(group);
    }

    private void OnClickPartySetupButton()
    {
        partySetupPanel.EnterPartySetupMode();
    }
}
