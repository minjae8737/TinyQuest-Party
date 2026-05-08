using System.Collections.Generic;
using UnityEngine;

public class PartySetupPanel : UIPage
{
    [Header("=== References ===")]
    [SerializeField] private RectTransform panelGroup;
    [SerializeField] private RectTransform partySlotParent;
    [SerializeField] private RectTransform unitListSlotParent;
    
    [Header("=== Prefabs ===")]
    [SerializeField] private GameObject partySlotPrefab;
    [SerializeField] private GameObject unitListSlotPrefab;

    [Header("=== Resources ===")] 
    [SerializeField] private Sprite emptyPartySlotSprite;
    [SerializeField] private Sprite[] starGradeSprites;

    [Header("=== Caching ===")]
    private Vector2 panelGroupOriginPos;
    
    private List<PartySlotUI> partySlotUis;
    private List<UnitListSlotUI> unitListSlotUis;

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
        List<UnitController> unitDatas = UnitManager.Instance.TeamUnitDic[TeamType.Player];
        unitListSlotUis = new();
        
        foreach (UnitController unitController in unitDatas)
        {
            CreateUnitListItem(unitController);
        }

        panelGroupOriginPos = panelGroup.anchoredPosition;
        
        UnitManager.Instance.OnPartyChanged += RefreshPartyPanel;
        UnitManager.Instance.OnPartyChanged += RefreshUnitListPanel;
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
        List<PartyUnitDTO> partyUnitDtos = UnitManager.Instance.GetPartyData();

        for (int i = 0; i < partyUnitDtos.Count; i++)
        {
            PartyUnitDTO partyUnitDto = partyUnitDtos[i];
            
            partySlotUis[i].SetSlot(partyUnitDto, starGradeSprites[partyUnitDto.starGrade], i);
        }
    }

    #endregion

    #region UnitListPanel

    public void CreateUnitListItem(UnitController unitController)
    {
        GameObject unitListSlotObj = Instantiate(unitListSlotPrefab, unitListSlotParent);

        if (!unitListSlotObj.TryGetComponent<UnitListSlotUI>(out var unitListSlot))
        {
            Debug.LogError("CreateUnitListItemUI Fail. unitName : " + unitController.Model.Data.UnitName);
            return;
        }

        unitListSlotUis.Add(unitListSlot);
        unitListSlot.SetSlot(unitController.Model, starGradeSprites[unitController.Model.StarGrade]);
    }

    public void RefreshUnitListPanel()
    {
        List<UnitController> unitDatas = UnitManager.Instance.TeamUnitDic[TeamType.Player];
        int count = Mathf.Min(unitListSlotUis.Count, unitDatas.Count);

        for (int i = 0; i < count; i++)
        {
            Unit model = unitDatas[i].Model;
            unitListSlotUis[i].SetSlot(model, starGradeSprites[model.StarGrade]);
        }
    }

    #endregion
}
