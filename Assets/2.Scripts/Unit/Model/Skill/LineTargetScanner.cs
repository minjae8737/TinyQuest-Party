using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Scan/Line")]
public class LineTargetScanner : SkillTargetScanner
{
    public override List<UnitController> Scan(UnitController caster, SkillData skillData)
    {
        LineTargetData targetData = (LineTargetData)skillData.TargetData;
        Vector2 casterPos = caster.transform.position;
        Vector2 forward = caster.Forward;
        List<UnitController> targets = new List<UnitController>();
        
        int count = Physics2D.OverlapCircle(casterPos, targetData.GetSkillDistance(), contactFilter, enemies);
        
        // 가장 가까운 대상 탐색
        Collider2D nearestEnemy = FindNearestEnemy(casterPos, count);
        if (nearestEnemy == null) return targets;

        targetPos = nearestEnemy.transform.position;
        if (!targetData.IsInMaxDistance(casterPos, targetPos)) return targets;

        Vector2 boxArea = targetData.GetBoxArea();
        float angle = targetData.GetAngle(casterPos, targetPos, forward);

        // 스킬 범위 중심으로 재탐색
        Vector2 toTargetDir = (targetPos - casterPos).normalized;
        Vector2 point = casterPos + toTargetDir * (boxArea.x * 0.5f);

        int enemyCounts = Physics2D.OverlapBox(point, boxArea, angle, contactFilter, enemies);

        for (int i = 0; i < enemyCounts; i++)
        {
            Collider2D enemy = enemies[i];

            if (enemy.TryGetComponent<UnitController>(out var controller))
            {
                targets.Add(controller);
            }
        }

        return targets;
    }
}
