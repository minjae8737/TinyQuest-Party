using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public DragItemUI DragItemUI { get; private set; }
    
    [Header("=== Canvas References ===")]
    [SerializeField] private RectTransform worldCanvasRect;
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private PartySetupPanel partySetupPanel;
    
    [Header("=== Unit HP Bar ===")]
    [SerializeField] private RectTransform unitHpBarParent;
    [SerializeField] private List<UnitHpBar> unitHpBars;

    [Header("=== Prefab ===")] 
    [SerializeField] private GameObject DragItemUIPrefab;

    private bool isDragged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Init()
    {
        isDragged = false;
        
        // DragItemUI
        GameObject dragItemObj = Instantiate(DragItemUIPrefab, canvasRect);
        
        if (!dragItemObj.TryGetComponent<DragItemUI>(out var dragItem))
        {
            Debug.LogError("Fail Create DragItemUI");
        }
        
        DragItemUI = dragItem;
        DragItemUI.SetActive(false);
        
        // PartySetupPanel
        partySetupPanel.Init();
    }

    public UnitHpBar GetUnitHpBar()
    {
        GameObject hpBarObj = PoolManager.Instance.Get(ObjType.UnitHpBar);
        if (hpBarObj == null) return null;
        
        hpBarObj.TryGetComponent<UnitHpBar>(out var hpBar);

        if (hpBar != null)
        {
            unitHpBars.Add(hpBar);
            hpBar.transform.SetParent(unitHpBarParent, false);
        }
        
        return hpBar;
    }

    #region PartySetupPanel

    public void OpenPartySetupPanel()
    {
        partySetupPanel.gameObject.SetActive(true);
    }

    public void OffPartySetupPanel()
    {
        partySetupPanel.gameObject.SetActive(false);
    }

    #endregion

    #region DragItemUI
    
    public DragItemUI GetDragItem()
    {
        return DragItemUI;
    }

    #endregion
}
