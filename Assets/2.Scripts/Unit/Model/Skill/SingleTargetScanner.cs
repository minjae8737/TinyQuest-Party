using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Scan/Single")]
public class SingleTargetScanner : SkillTargetScanner
{
    public override SkillScanResult Scan(UnitController caster, SkillTargetData targetData)
    {
        SkillScanResult scanResult = new SkillScanResult();
        SingleTargetData singleTargetData = (SingleTargetData)targetData;
        Vector2 casterPos = caster.transform.position;
        Vector2 forward = caster.Forward;
        List<UnitController> targets = new List<UnitController>();

        targets = new(UnitManager.Instance.Units);
        
        // 범위 체크
        targets.RemoveAll(u => !singleTargetData.IsInRange(casterPos, u.transform.position, forward));

        // 필터 적용
        SelectActiveUnit(targets); // 활성화된 Unit만 선택
        targets = ApplyTeamFilter(singleTargetData, caster, targets);
        targets = ApplyConditionFilter(singleTargetData, targets);
        targets = ApplySelect(singleTargetData, targets);

        UnitController nearestTarget = FindNearestTarget(casterPos, targets);
        targets.Clear();
        targets.Add(nearestTarget);

        scanResult.Targets = targets;
        scanResult.PrimaryTarget = nearestTarget;

        return scanResult;
    }
}