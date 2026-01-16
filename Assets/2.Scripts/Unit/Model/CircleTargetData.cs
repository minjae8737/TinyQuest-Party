using UnityEngine;

[CreateAssetMenu(fileName = "CircleTargetData_(Character)_(SkillOrder)", menuName = "Skill/TargetData/Circle")]
public class CircleTargetData : SkillTargetData
{
    public override SkillTargetType TargetType => SkillTargetType.Circle;

    public float MaxDistance; // 최대 사거리
    public float Radius;      // 스킬 범위

    public bool IsInMaxDistance(Vector2 myPos, Vector2 centerPos)
    {
        return IsInDist(myPos, centerPos, MaxDistance);
    }
    
    public bool IsInMaxRange(Vector2 centerPos, Vector2 targetPos)
    {
        return IsInDist(centerPos, targetPos, Radius);
    }
}
