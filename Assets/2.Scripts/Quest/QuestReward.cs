using System;

public enum RewardType
{
    None, Gold, Exp,
}

[Serializable]
public class QuestReward
{
    public RewardType Type;
    public long Amount;

    public void Provide()
    {
        switch (Type)
        {
            case RewardType.Gold:
                CurrencyManager.Instance.AddGold(Amount);
                break;
            case RewardType.Exp:
                CurrencyManager.Instance.AddExp(Amount);
                break;
        }
    }
}
