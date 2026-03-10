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

        targets = new(UnitManager.Instance.Units);

        // 필터 적용
        targets = ApplyTeamFilter(circleTargetData, caster, targets);
        
        // 가장 가까운 대상 탐색
        UnitController nearestTarget = FindNearestTarget(casterPos, targets);
        if (nearestTarget == null) return scanResult;
        
        // nearestTarget 중심으로 재탐색
        Vector2 nearestEnemyPos = nearestTarget.transform.position;
        if (!circleTargetData.IsInMaxDistance(casterPos, nearestEnemyPos)) return scanResult;
        
        targets.RemoveAll(u => !circleTargetData.IsInMaxRange(nearestEnemyPos, u.transform.position));
        
        // 필터 적용
        SelectActiveUnit(targets); // 활성화된 Unit만 선택
        targets = ApplyConditionFilter(circleTargetData, targets);
        targets = ApplySelect(circleTargetData, targets);
        
        scanResult.Targets = targets;
        scanResult.PrimaryTarget = nearestTarget;

        return scanResult;
    }
}
