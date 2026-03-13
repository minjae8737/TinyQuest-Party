using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    [Header("=== Canvas References ===")]
    [SerializeField] private RectTransform worldCanvasRect;
    [SerializeField] private PartySetupPanel partySetupPanel;
    
    
    [Header("=== Unit HP Bar ===")]
    [SerializeField] private RectTransform unitHpBarParent;
    [SerializeField] private List<UnitHpBar> unitHpBars;


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
}
