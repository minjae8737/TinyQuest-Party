using System;
using UnityEngine;

public enum TargetLayerType
{
    Enemy,
    Ally,
    Self,
    All
}

public enum TargetSortType
{
    None,
    LowHp,
    LowHpPercent,
    HighHp,
    HighHpPercent,
}

[Serializable]
public abstract class SkillTargetData : ScriptableObject
{
    [Header("=== Basic Info ===")]
    public TargetLayerType TargetLayer;
    public TargetSortType SortType;
    [Min(0)] public int MaxTargetCount;

    protected bool IsInAngle(Vector2 myPos, Vector2 targetPos, Vector2 forward, float angle)
    {
        Vector2 toTarget = targetPos - myPos;
        return Math.Abs(Vector2.SignedAngle(forward, toTarget)) <= angle * 0.5f; // forward 방향 기준 한쪽 각도
    }

    protected bool IsInDist(Vector2 myPos, Vector2 targetPos, float targetDist)
    {
        Vector2 diff = targetPos - myPos;
        return diff.sqrMagnitude <= targetDist * targetDist; // magnitude 보다 가벼움
    }

    public abstract float GetSkillDistance();
}