using UnityEngine;

[CreateAssetMenu(fileName = "SingleTargetData_(Character)_(SkillOrder)", menuName = "Skill/TargetData/Single")]
public class SingleTargetData : DirectTargetData
{
    [Header("=== Range Info ===")]
    public float MaxDistance;

    public override bool IsInRange(Vector2 casterPos, Vector2 targetPos, Vector2 forward)
    {
        return IsInDist(casterPos, targetPos, MaxDistance);
    }

    public override float GetSkillDistance()
    {
        return MaxDistance;
    }
}
