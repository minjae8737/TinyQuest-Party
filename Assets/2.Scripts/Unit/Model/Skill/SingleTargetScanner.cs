using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Scan/Single")]
public class SingleTargetScanner : SkillTargetScanner
{
    public override List<UnitController> Scan(UnitController caster, SkillData skillData)
    {
        SingleTargetData targetData = (SingleTargetData)skillData.TargetData;
        Vector2 casterPos = caster.transform.position;
        Vector2 forward = caster.Forward;
        List<UnitController> targets = new List<UnitController>();

        int count = Physics2D.OverlapCircle(casterPos, targetData.GetSkillDistance(), contactFilter, enemies);
        Collider2D nearestEnemy = FindNearestEnemy(casterPos, count);
        if (nearestEnemy == null || !targetData.IsInRange(casterPos, nearestEnemy.transform.position, forward)) return targets;
        
        targetPos = nearestEnemy.transform.position;

        if (nearestEnemy.TryGetComponent<UnitController>(out var controller))
        {
            targets.Add(controller);
        }


        return targets;
    }
}