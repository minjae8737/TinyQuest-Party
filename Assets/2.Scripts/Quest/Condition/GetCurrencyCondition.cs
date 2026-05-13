using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GetCurrency_000",menuName = "Quest/Condition/GetCurrency")]
public class GetCurrencyCondition : QuestCondition
{
    public CurrencyType Type;
    public long RequiredAmount;
    
    private const string KeyPrefix = "Add"; // AddGold, AddExp
    private string GetKey () => KeyPrefix + (Type == CurrencyType.Gold ? "Gold" : "Exp");
    
    public override string GetDesc(QuestProgress progress)
    {
        string currencyName = Type == CurrencyType.Gold ? "골드" : "경험치";
        string requiredAmountStr = UIManager.Instance.NumberFormatter(RequiredAmount);
        string countStr = UIManager.Instance.NumberFormatter(progress.GetCount(GetKey()));
        
        return $"{currencyName} 모으기 ({countStr}/{requiredAmountStr})";
    }

    public override bool isSatisfied(QuestProgress progress)
    {
        return progress.GetCount(GetKey()) >= RequiredAmount;
    }
}