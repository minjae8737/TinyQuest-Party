using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GetCurrency_000",menuName = "Quest/Condition/GetCurrency")]
public class GetCurrencyCondition : QuestCondition
{
    public CurrencyType Type;
    public long RequiredAmount;
    
    public override string GetDesc()
    {
        throw new NotImplementedException();
    }

    public override bool isSatisfied(QuestProgress progress)
    {
        return progress.Count >= RequiredAmount;
    }
}