using System;
using System.Collections.Generic;
using UnityEngine;

public enum CurrencyType
{
    Gold,
    Exp,
}

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    [SerializeField] private List<CurrencyData> Datas;
    private Dictionary<CurrencyType, CurrencyData> dataDic;
    public IReadOnlyDictionary<CurrencyType, CurrencyData> DataDic => dataDic;
    
    private const long MaxValue = 9999999999999999L;
    public long Gold { get; private set; }
    public long Exp { get; private set; }

    public event Action OnGoldChanged;
    public event Action OnExpChanged;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        OnGoldChanged += UIManager.Instance.RefreshGoldPanel;
        OnExpChanged += UIManager.Instance.RefreshExpPanel;
    }

    private void OnDisable()
    {
        OnGoldChanged -= UIManager.Instance.RefreshGoldPanel;
        OnExpChanged -= UIManager.Instance.RefreshExpPanel;
    }

    public void Init()
    {
        Gold = 0L;
        Exp = 0L;
        dataDic = new();

        foreach (CurrencyData data in Datas)
        {
            if (!dataDic.TryAdd(data.Type, data))
            {
                Debug.Log($"{data.Type} Data is Duplicated");
            }
        }
    }

    #region Gold

    private bool HasEnoughGold(long amount)
    {
        return amount >= Gold;
    }
    
    public void AddGold(long amount)
    {
        Gold += amount;
        Gold = Gold >= MaxValue ? MaxValue : Gold;
        OnGoldChanged?.Invoke();
    }

    public bool SpendGold(long amount)
    {
        if (!HasEnoughGold(amount)) return false;
        
        Gold -= amount;
        OnGoldChanged?.Invoke();

        return true;
    }
    
    #endregion

    #region Exp

    private bool HasEnoughExp(long amount)
    {
        return amount >= Exp;
    }

    public void AddExp(long amount)
    {
        Exp += amount;
        Exp = Exp >= MaxValue ? MaxValue : Exp;
        OnExpChanged?.Invoke();
    }

    public bool SpendExp(long amount)
    {
        if (!HasEnoughExp(amount)) return false;
        
        Exp -= amount;
        OnExpChanged?.Invoke();

        return true;
    }

    #endregion
}
