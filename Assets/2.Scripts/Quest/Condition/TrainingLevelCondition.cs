using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Traininglevel_000",menuName = "Quest/Condition/TrainingLevel")]
public class TrainingLevelCondition: QuestCondition
{
    public TrainingType Type;
    public int RequiredTrainingLevel; // 
    public int RequiredTypeLevel;     //

    public override string GetDesc()
    {
        string desc = $" ({RequiredTrainingLevel}/)";
        
        throw new NotImplementedException();
    }
    
    public override bool isSatisfied(QuestProgress progress)
    {
        int statLevel = TrainingManager.Instance.GetStatLevel(Type, RequiredTrainingLevel);
        
        return statLevel > RequiredTypeLevel;
    }
}