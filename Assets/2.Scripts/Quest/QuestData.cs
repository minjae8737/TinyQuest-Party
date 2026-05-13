using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestData_000", menuName = "Quest/Data")]
public class QuestData : ScriptableObject
{
    public int index;
    public string Title;

    [Header("=== Condition ===")]
    public QuestCondition Condition;

    [Header("=== Rewards ===")] 
    public QuestReward Reward;
}
