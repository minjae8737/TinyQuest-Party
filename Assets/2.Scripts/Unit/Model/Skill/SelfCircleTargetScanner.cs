using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Scan/SelfCircle")]
public class SelfCircleTargetScanner : SkillTargetScanner
{
    public override SkillScanResult Scan(UnitController caster, SkillTargetData targetData)
    {
        SkillScanResult scanResult = new SkillScanResult();
        CircleTargetData circleTargetData = (CircleTargetData)targetData;
        Vector2 casterPos = caster.transform.position;
        Vector2 forward = caster.Forward;
        List<UnitController> targets = new List<UnitController>();

        targets = new(UnitManager.Instance.Units);

        targets.RemoveAll(u => !circleTargetData.IsInMaxRange(casterPos, u.transform.position));
        
        // 필터 적용
        SelectActiveUnit(targets); // 활성화된 Unit만 선택
        targets = ApplyTeamFilter(circleTargetData, caster, targets);
        targets = ApplyConditionFilter(circleTargetData, targets);
        targets = ApplySelect(circleTargetData, targets);

        UnitController nearestTarget = FindNearestTarget(casterPos, targets);
        
        scanResult.Targets = targets;
        scanResult.PrimaryTarget = caster;

        return scanResult;
    }
}