using System;
using UnityEngine;

[CreateAssetMenu(fileName = "KillUnit_000",menuName = "Quest/Condition/KillUnit")]
public class KillUnitCondition : QuestCondition
{
    public int RequiredCount;
    private const string Key = "KillUnit";

    public override string GetDesc(QuestProgress progress)
    {
        return $"유닛 처치하기 ({progress.GetCount(Key)}/{RequiredCount})";
    }

    public override bool isSatisfied(QuestProgress progress)
    {
        return progress.GetCount(Key) >= RequiredCount;
    }
}
