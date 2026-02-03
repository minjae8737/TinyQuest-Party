using UnityEngine;

[CreateAssetMenu(fileName = "ConeTargetData_(Character)_(SkillOrder)", menuName = "Skill/TargetData/Cone")]
public class ConeTargetData : DirectTargetData
{
    public override SkillTargetType TargetType => SkillTargetType.Cone;

    [Range(0, 180)] public float Angle; 
    public float Length;

    public override bool IsInRange(Vector2 myPos, Vector2 targetPos , Vector2 forward)
    {
        return IsInAngle(myPos, targetPos, forward, Angle) && IsInDist(myPos, targetPos, Length); 
    }

    public override float GetSkillDistance()
    {
        return Length;
    }
}
