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

    public event Action<long> OnGoldChanged;
    public event Action<long> OnExpChanged;
    public event Action<string, long> OnAddGold;
    public event Action<string, long> OnAddExp;

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

    public void Init(CurrencySaveData saveData = null)
    {
        Gold = 0L;
        Exp = 0L;
        ApplySaveData(saveData);
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
        return Gold >= amount;
    }

    public void AddGold(long amount)
    {
        Gold += amount;
        Gold = Gold >= MaxValue ? MaxValue : Gold;
        OnGoldChanged?.Invoke(amount);
        OnAddGold?.Invoke("AddGold", amount);
    }

    public bool SpendGold(long amount)
    {
        if (!HasEnoughGold(amount)) return false;

        Gold -= amount;
        OnGoldChanged?.Invoke(-amount);

        return true;
    }

    #endregion

    #region Exp

    private bool HasEnoughExp(long amount)
    {
        return Exp >= amount;
    }

    public void AddExp(long amount)
    {
        Exp += amount;
        Exp = Exp >= MaxValue ? MaxValue : Exp;
        OnExpChanged?.Invoke(amount);
        OnAddExp?.Invoke("AddExp", amount);
    }

    public bool SpendExp(long amount)
    {
        if (!HasEnoughExp(amount)) return false;

        Exp -= amount;
        OnExpChanged?.Invoke(-amount);

        return true;
    }

    #endregion

    #region SaveData

    public CurrencySaveData GetCurrencySaveData()
    {
        return new CurrencySaveData(Gold, Exp);
    }

    private void ApplySaveData(CurrencySaveData saveData)
    {
        if (saveData != null)
        {
            Gold = saveData.Gold;
            Exp = saveData.Exp;
        }
    }

    #endregion
    
}