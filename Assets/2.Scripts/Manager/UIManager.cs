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
}
