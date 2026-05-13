using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Traininglevel_000",menuName = "Quest/Condition/TrainingLevel")]
public class TrainingLevelCondition: QuestCondition
{
    public TrainingType Type;
    public int RequiredTrainingLevel; // 
    public int RequiredTypeLevel;     //

    public override string GetDesc(QuestProgress progress)
    {
        return $"{RequiredTrainingLevel+1}단계\n" +
               $"{GetStatName()} Lv{RequiredTypeLevel} 달성하기 ({GetStatLevel()}/{RequiredTypeLevel})";
    }
    
    public override bool isSatisfied(QuestProgress progress)
    {
        int statLevel = GetStatLevel();
        
        return statLevel >= RequiredTypeLevel;
    }

    private int GetStatLevel()
    {
        return TrainingManager.Instance.GetStatLevel(Type, RequiredTrainingLevel);
    }

    private string GetStatName()
    {
        switch (Type)
        {
            case TrainingType.Attack:
                return "공격력";
            case TrainingType.Defence:
                return "방어력";
            case TrainingType.Health:
                return "생명력";
        }

        return "None";
    }
}