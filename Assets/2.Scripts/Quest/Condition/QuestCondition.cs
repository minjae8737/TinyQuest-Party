using UnityEngine;

public abstract class QuestCondition : ScriptableObject
{
    public abstract bool isSatisfied(QuestProgress progress);
}
