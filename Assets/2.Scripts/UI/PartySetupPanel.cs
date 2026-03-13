using System.Collections.Generic;
using UnityEngine;

public class PartySetupPanel : MonoBehaviour
{

    [Header("=== References ===")] 
    [SerializeField] private RectTransform slotParent;
    [SerializeField] private RectTransform listParent;
    
    [Header("=== Prefabs ===")]
    [SerializeField] private GameObject unitSlotPrefab;
    [SerializeField] private GameObject unitListItemPrefab;
    
    public void Init()
    {
        // PartyPanel
        
        // UnitListPanel
        List<UnitName> unitNames = UnitManager.Instance.UnitNamesByTeam[TeamType.Player];


        UnitManager.Instance.OnPartyChanged += RefreshPartyPanel;
        UnitManager.Instance.OnPartyChanged += RefreshUnitListPanel;
    }

    #region PartyPanel

    public void RefreshPartyPanel()
    {
        
    }

    #endregion

    #region UnitListPanel

    public void RefreshUnitListPanel()
    {
        
    }

    public void CreateUnitListItem()
    {
        
    }

    #endregion
}
