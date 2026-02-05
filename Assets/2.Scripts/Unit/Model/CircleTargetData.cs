using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CircleTargetData_(Character)_(SkillOrder)", menuName = "Skill/TargetData/Circle")]
public class CircleTargetData : SkillTargetData
{
    public override SkillTargetType TargetType => SkillTargetType.Circle;

    public float MaxDistance; // 최대 사거리
    public float Radius;      // 스킬 범위
    public bool IsTargetSelf => MaxDistance == 0; // MaxDistance가 0일시 시전 캐릭터 중심범위로 스킬 시전
    
    public bool IsInMaxDistance(Vector2 myPos, Vector2 centerPos)
    {
        float distance = Mathf.Max(MaxDistance, Radius);
        return IsInDist(myPos, centerPos, distance);
    }
    
    public bool IsInMaxRange(Vector2 centerPos, Vector2 targetPos)
    {
        return IsInDist(centerPos, targetPos, Radius);
    }

    public override float GetSkillDistance()
    {
        // MaxDistance가 0일때 Radius범위 안에 타겟이 있어도 공격하지 않아
        // 둘중 큰값 사거리 안에 있을경우 공격하도록 Max() 사용
        return Mathf.Max(MaxDistance, Radius); 
    }
}
