using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CircleTargetData_(Character)_(SkillOrder)", menuName = "Skill/TargetData/Circle")]
public class CircleTargetData : SkillTargetData
{
    [Header("=== Range Info ===")]
    public float MaxDistance; // 최대 사거리
    public float Radius;      // 스킬 범위
    
    public bool IsInMaxDistance(Vector2 casterPos, Vector2 centerPos)
    {
        float distance = Mathf.Max(MaxDistance, Radius);
        return IsInDist(casterPos, centerPos, distance);
    }
    
    public bool IsInMaxRange(Vector2 centerPos, Vector2 targetPos)
    {
        return IsInDist(centerPos, targetPos, Radius);
    }

    public override float GetSkillDistance()
    {
        return Mathf.Max(MaxDistance, Radius); 
    }
}
