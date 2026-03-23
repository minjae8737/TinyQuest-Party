using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    [Header("=== Canvas References ===")]
    [SerializeField] private RectTransform worldCanvasRect;

    [SerializeField] private RectTransform damageTextRect;
    public RectTransform DamageTextRect => damageTextRect;
    [SerializeField] private RectTransform canvasRect;
    
    
    [SerializeField] private GameObject mainButtonGroup;
    [SerializeField] private PartySetupPanel partySetupPanel;

    [SerializeField] private RectTransform GoldPanel;
    [SerializeField] private RectTransform ExpPanel;
    private RectTransform GoldPanelIcon;
    private RectTransform ExpPanelIcon;

    [Header("=== Prefab ===")] 
    [SerializeField] private GameObject DragItemUIPrefab;

    private DragItemUI DragItemUI;
    [HideInInspector] public DragContext DragContext;
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
        
        // MainButtonGroup
        mainButtonGroup.SetActive(true);
        
        // PartySetupPanel
        partySetupPanel.Init();
        
        // MainTopGroup
        GoldPanelIcon = GoldPanel.GetChild(0).GetComponent<RectTransform>();
        ExpPanelIcon = ExpPanel.GetChild(0).GetComponent<RectTransform>();
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

    #region Drag
    
    public DragItemUI GetDragItem()
    {
        return DragItemUI;
    }

    #endregion

    // MainTopGroup 임시 네이밍
    #region MainTopGroup

    public Vector3 GetCurrencyPos(CurrencyType type)
    {
        Vector3 screenPos = Vector3.zero;
        
        switch (type)
        {
            case CurrencyType.Gold:
                screenPos= RectTransformUtility.WorldToScreenPoint(null, GoldPanelIcon.position);
                break;
            case CurrencyType.Exp:
                screenPos= RectTransformUtility.WorldToScreenPoint(null, ExpPanelIcon.position);
                break;
        }

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        worldPos.z = 0f;

        return worldPos;
    }

    public Vector3 GetInventoryPos()
    {
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(null, GoldPanelIcon.position); //TODO GoldPanelIcon 을 inventoryButton으로 바꿀 것 
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        worldPos.z = 0f;

        return worldPos;
    }

    #endregion
}
