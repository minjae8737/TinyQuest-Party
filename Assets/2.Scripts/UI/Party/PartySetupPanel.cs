using System.Collections.Generic;
using UnityEngine;

public class PartySetupPanel : UIPage
{
    [Header("=== References ===")] 
    [SerializeField] private RectTransform partySlotParent;
    [SerializeField] private RectTransform unitListSlotParent;
    
    [Header("=== Prefabs ===")]
    [SerializeField] private GameObject partySlotPrefab;
    [SerializeField] private GameObject unitListSlotPrefab;

    [Header("=== Resources ===")] 
    [SerializeField] private Sprite emptyPartySlotSprite;
    
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
        Dictionary<UnitName, UnitData> unitDatas = UnitManager.Instance.UnitDataDic;
        unitListSlotUis = new();
        
        foreach (KeyValuePair<UnitName, UnitData> unitData in unitDatas)
        {
            if (unitData.Value.TeamType == TeamType.Player)
            {
                CreateUnitListItem(unitData.Value);
            }
        }
        
        UnitManager.Instance.OnPartyChanged += RefreshPartyPanel;
        UnitManager.Instance.OnPartyChanged += RefreshUnitListPanel;
    }
    
    public override void Show()
    {
        gameObject.SetActive(true);
        AudioManager.Instance.PlaySfx(Sfx.UIOpen);
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
        List<UnitData> partyData = UnitManager.Instance.GetPartyData();

        for (int i = 0; i < partyData.Count; i++)
        {
            Sprite unitSprite = partyData[i] == null ? emptyPartySlotSprite : partyData[i].Icon;
            UnitName unitName = partyData[i] == null ? UnitName.None : partyData[i].UnitName;
            partySlotUis[i].SetSlot(unitSprite, unitName, i);
        }
    }

    #endregion

    #region UnitListPanel

    public void CreateUnitListItem(UnitData unitData)
    {
        GameObject unitListSlotObj = Instantiate(unitListSlotPrefab, unitListSlotParent);

        if (!unitListSlotObj.TryGetComponent<UnitListSlotUI>(out var unitListSlot))
        {
            Debug.LogError("CreateUnitListItemUI Fail. unitName : " + unitData.UnitName);
            return;
        }

        unitListSlotUis.Add(unitListSlot);
        unitListSlot.SetSlot(unitData.Icon, unitData.UnitName);
    }

    public void RefreshUnitListPanel()
    {
        
    }

    #endregion
}
