using UnityEngine;

[CreateAssetMenu(fileName = "KillUnit_000",menuName = "Quest/Condition/KillUnit")]
public class KillUnitCondition : QuestCondition
{
    public int RequiredCount;
    
    public override bool isSatisfied(QuestProgress progress)
    {
        return progress.Count >= RequiredCount;
    }
}
