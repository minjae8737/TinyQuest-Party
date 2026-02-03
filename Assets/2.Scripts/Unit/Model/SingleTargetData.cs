using UnityEngine;

[CreateAssetMenu(fileName = "SingleTargetData_(Character)_(SkillOrder)", menuName = "Skill/TargetData/Single")]
public class SingleTargetData : DirectTargetData
{
    public override SkillTargetType TargetType => SkillTargetType.Single;

    public float MaxDistance;

    public override bool IsInRange(Vector2 myPos, Vector2 targetPos, Vector2 forward)
    {
        return IsInDist(myPos, targetPos, MaxDistance);
    }

    public override float GetSkillDistance()
    {
        return MaxDistance;
    }
}
