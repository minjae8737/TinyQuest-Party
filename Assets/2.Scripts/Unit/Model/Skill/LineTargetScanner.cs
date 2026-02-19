using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Scan/Line")]
public class LineTargetScanner : SkillTargetScanner
{
    public override SkillScanResult Scan(UnitController caster, SkillTargetData targetData)
    {
        SkillScanResult scanResult = new SkillScanResult();
        LineTargetData lineTargetData = (LineTargetData)targetData;
        Vector2 casterPos = caster.transform.position;
        Vector2 forward = caster.Forward;
        List<UnitController> targets = new List<UnitController>();
        
        int count = Physics2D.OverlapCircle(casterPos, targetData.GetSkillDistance(), contactFilter, enemies);
        GetUnitController(count, targets);

        // 필터 적용
        targets = ApplyTeamFilter(lineTargetData, caster, targets);
        targets = ApplyConditionFilter(lineTargetData, targets);
        targets = ApplySelect(lineTargetData, targets);
        
        // 가장 가까운 대상 탐색
        UnitController nearestTarget = FindNearestTarget(casterPos, targets);
        if (nearestTarget == null) return scanResult;

        Vector2 nearestEnemyPos = nearestTarget.transform.position;
        if (!lineTargetData.IsInMaxDistance(casterPos, nearestEnemyPos)) return scanResult;

        Vector2 boxArea = lineTargetData.GetBoxArea();
        float angle = lineTargetData.GetAngle(casterPos, nearestEnemyPos, forward);

        // 스킬 범위 중심으로 재탐색
        Vector2 toTargetDir = (nearestEnemyPos - casterPos).normalized;
        Vector2 point = casterPos + toTargetDir * (boxArea.x * 0.5f);

        int enemyCounts = Physics2D.OverlapBox(point, boxArea, angle, contactFilter, enemies);
        targets.Clear();
        GetUnitController(enemyCounts, targets);

        scanResult.Targets = targets;
        scanResult.PrimaryTarget = nearestTarget;
        
        return scanResult;
    }
}
