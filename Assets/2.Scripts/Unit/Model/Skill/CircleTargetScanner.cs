using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Scan/Circle")]
public class CircleTargetScanner : SkillTargetScanner
{
    public override SkillScanResult Scan(UnitController caster, SkillTargetData targetData)
    {
        SkillScanResult scanResult = new SkillScanResult();
        CircleTargetData circleTargetData  = (CircleTargetData)targetData;
        Vector2 casterPos = caster.transform.position;
        Vector2 forward = caster.Forward;
        List<UnitController> targets = new List<UnitController>();

        int count = Physics2D.OverlapCircle(casterPos, circleTargetData.GetSkillDistance(), contactFilter, enemies);
        // enemies 에서 UnitController 추출
        GetUnitController(count, targets);
        
        // 필터 적용
        targets = ApplyTeamFilter(circleTargetData, caster, targets);
        
        // 가장 가까운 대상 탐색
        UnitController nearestTarget = FindNearestTarget(casterPos, targets);
        if (nearestTarget == null) return scanResult;

        Vector2 nearestEnemyPos = nearestTarget.transform.position;
        if (!circleTargetData.IsInMaxDistance(casterPos, nearestEnemyPos)) return scanResult;

        // nearestTarget 중심으로 재탐색
        int enemyCounts = Physics2D.OverlapCircle(nearestEnemyPos, circleTargetData.Radius, contactFilter, enemies);
        targets.Clear();
        GetUnitController(enemyCounts, targets);
        
        // 필터 적용
        targets = ApplyTeamFilter(circleTargetData, caster, targets);
        targets = ApplyConditionFilter(circleTargetData, targets);
        targets = ApplySelect(circleTargetData, targets);
        
        scanResult.Targets = targets;
        scanResult.PrimaryTarget = nearestTarget;

        return scanResult;
    }
}
