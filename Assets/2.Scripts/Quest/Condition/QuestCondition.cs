using UnityEngine;

public abstract class QuestCondition : ScriptableObject
{
    public abstract string GetDesc(QuestProgress progress);
    public abstract bool isSatisfied(QuestProgress progress);
}
