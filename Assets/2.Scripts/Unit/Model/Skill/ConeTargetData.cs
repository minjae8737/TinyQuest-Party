using UnityEngine;

[CreateAssetMenu(fileName = "ConeTargetData_(Character)_(SkillOrder)", menuName = "Skill/TargetData/Cone")]
public class ConeTargetData : DirectTargetData
{
    public override SkillTargetType TargetType => SkillTargetType.Cone;

    [Range(0, 180)] public float Angle; 
    public float Length;

    public override bool IsInRange(Vector2 casterPos, Vector2 targetPos , Vector2 forward)
    {
        return IsInAngle(casterPos, targetPos, forward, Angle) && IsInDist(casterPos, targetPos, Length); 
    }

    public override float GetSkillDistance()
    {
        return Length;
    }
}
