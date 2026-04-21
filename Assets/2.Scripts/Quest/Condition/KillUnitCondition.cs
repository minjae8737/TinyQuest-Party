using System;
using UnityEngine;

[CreateAssetMenu(fileName = "KillUnit_000",menuName = "Quest/Condition/KillUnit")]
public class KillUnitCondition : QuestCondition
{
    public int RequiredCount;

    public override string GetDesc()
    {
        throw new NotImplementedException();
    }

    public override bool isSatisfied(QuestProgress progress)
    {
        return progress.Count >= RequiredCount;
    }
}
