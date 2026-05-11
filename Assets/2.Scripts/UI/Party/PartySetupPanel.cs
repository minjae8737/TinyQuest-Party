using System.Collections.Generic;
using UnityEngine;

public class PartySetupPanel : UIPage
{
    [Header("=== References ===")]
    [SerializeField] private RectTransform panelGroup;
    [SerializeField] private RectTransform partySlotParent;
    [SerializeField] private RectTransform unitSlotParent;
    
    [Header("=== Prefabs ===")]
    [SerializeField] private GameObject partySlotPrefab;
    [SerializeField] private GameObject unitSlotPrefab;

    [Header("=== Resources ===")] 
    [SerializeField] private Sprite emptyPartySlotSprite;
    [SerializeField] private Sprite[] starGradeSprites;

    [Header("=== Caching ===")]
    private Vector2 panelGroupOriginPos;
    
    private List<PartySlotUI> partySlotUis;
    private List<UnitSlotUI> unitListSlotUis;

    public void Init()
    {
        // PartyPanel
        partySlotUis = new();
        
        for (int i = 0; i < UnitManager.MaxPartySize; i++)
        {
            CreatePartySlot();
        }
        
        RefreshPartyPanel();
        
        // UnitListPanel
        unitListSlotUis = new();
        CreateUnitSlot();

        panelGroupOriginPos = panelGroup.anchoredPosition;
        
        UnitManager.Instance.OnPartyChanged += RefreshPartyPanel;
        // UnitManager.Instance.OnPartyChanged += RefreshUnitListPanel;
    }
    
    public override void Show()
    {
        gameObject.SetActive(true);
        AudioManager.Instance.PlaySfx(Sfx.UIOpen);
        UIEffect.SlideUp(panelGroup, panelGroupOriginPos, 50f, 0.7f);
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
        AudioManager.Instance.PlaySfx(Sfx.UIClose);
    }

    #region PartyPanel

    private void CreatePartySlot()
    {
        GameObject partySlotObj = Instantiate(partySlotPrefab, partySlotParent);
        
        if (!partySlotObj.TryGetComponent<PartySlotUI>(out var partySlot))
        {
            Debug.LogError("CreatePartySlotUI Fail");
            return;
        } 
        
        partySlotUis.Add(partySlot);
    }
    
    public void RefreshPartyPanel()
    {
        List<UnitSlotDTO> unitSlotDtos = UnitManager.Instance.GetPartyData();
        
        for (int i = 0; i < unitSlotDtos.Count; i++)
        {
            UnitSlotDTO unitSlotDto = unitSlotDtos[i];
            
            partySlotUis[i].SetSlot(unitSlotDto, starGradeSprites[unitSlotDto.StarGrade], i);
        }
    }

    #endregion

    #region UnitListPanel

    public void CreateUnitSlot()
    {
        List<UnitSlotDTO> unitSlotDtos = UnitManager.Instance.GetPlayerUnitSlotDTO();

        foreach (UnitSlotDTO unitSlotDto in unitSlotDtos)
        {
            GameObject unitSlotObj = Instantiate(unitSlotPrefab, unitSlotParent);

            if (!unitSlotObj.TryGetComponent<UnitSlotUI>(out var unitSlot))
            {
                Debug.LogError($"CreateUnitListItemUI Fail. Unitname : {unitSlotDto.UnitName}");
                return;
            }

            unitListSlotUis.Add(unitSlot);
            unitSlot.SetSlot(unitSlotDto, starGradeSprites[unitSlotDto.StarGrade]);
        }
    }

    public void RefreshUnitListPanel()
    {

    }

    #endregion
}
