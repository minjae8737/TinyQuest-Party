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
}
