using UnityEngine;

[CreateAssetMenu(fileName = "LineTargetData_(Character)_(SkillOrder)", menuName = "Skill/TargetData/Line")]
public class LineTargetData : SkillTargetData
{
    public override SkillTargetType TargetType => SkillTargetType.Line;

    public float MaxDistance; // 최대 사거리
    public float Width; // 가로
    public float Length; // 세로

    public bool IsInMaxDistance(Vector2 myPos, Vector2 centerPos)
    {
        return IsInDist(myPos, centerPos, MaxDistance + Length);
    }

    public Vector2 GetBoxArea()
    {
        return new Vector2(Width, Length);
    }

    public float GetAngle(Vector2 myPos, Vector2 targetPos,Vector2 forward)
    {
        Vector2 toTarget = targetPos - myPos;
        return Vector2.Angle(forward, toTarget);
    }
}
