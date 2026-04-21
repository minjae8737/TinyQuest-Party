using UnityEngine;

public abstract class QuestCondition : ScriptableObject
{
    public abstract string GetDesc();
    public abstract bool isSatisfied(QuestProgress progress);
}
