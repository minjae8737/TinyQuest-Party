using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Scan/Circle")]
public class CircleTargetScanner : SkillTargetScanner
{
    public override List<UnitController> Scan(UnitController caster, SkillData skillData)
    {
        CircleTargetData targetData = (CircleTargetData)skillData.TargetData;
        Vector2 casterPos = caster.transform.position;
        Vector2 forward = caster.Forward;
        List<UnitController> targets = new List<UnitController>();

        int count = Physics2D.OverlapCircle(casterPos, targetData.GetSkillDistance(), contactFilter, enemies);

        // 가장 가까운 대상 탐색
        Collider2D nearestEnemy = FindNearestEnemy(casterPos, count);
        if (nearestEnemy == null) return targets;

        targetPos = nearestEnemy.transform.position;
        if (!targetData.IsInMaxDistance(casterPos, targetPos)) return targets;

        // target 중심으로 재탐색
        int enemyCounts = Physics2D.OverlapCircle(targetPos, targetData.Radius, contactFilter, enemies);
        
        for (int i = 0; i < enemyCounts; i++)
        {
            Collider2D enemy = enemies[i];

            // if (!targetData.IsInMaxRange(targetPos, enemy.transform.position)) continue;

            if (enemy.TryGetComponent<UnitController>(out var controller))
            {
                targets.Add(controller);
            }
        }

        return targets;
    }
}
