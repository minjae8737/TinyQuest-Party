
using UnityEngine;

public class DroppedItem_Currency : DroppedItemData
{
    public CurrencyType Type;
    public long Amount;

    public override void OnPickup()
    {
        switch (Type)
        {
            case CurrencyType.Gold:
                CurrencyManager.Instance.AddGold(Amount);
                break;
            case CurrencyType.Exp:
                CurrencyManager.Instance.AddExp(Amount);
                break;
        }
    }
}
