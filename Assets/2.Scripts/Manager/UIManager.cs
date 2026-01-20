using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [SerializeField] private RectTransform worldCanvasRect;

    [SerializeField] private RectTransform unitHpBarsRect;
    [SerializeField] private List<UnitHpBar> unitHpBars;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public UnitHpBar GetUnitHpBar()
    {
        GameObject hpBarObj = PoolManager.Instance.Get(ObjType.UnitHpBar);
        if (hpBarObj == null) return null;
        hpBarObj.TryGetComponent<UnitHpBar>(out var hpBar);

        if (hpBar != null)
        {
            unitHpBars.Add(hpBar);
            hpBar.transform.parent = unitHpBarsRect;
        }
        
        return hpBar;
    }
}
