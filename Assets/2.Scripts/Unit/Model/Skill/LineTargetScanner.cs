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
        
        targets = new(UnitManager.Instance.Units);

        targets.RemoveAll(u => !lineTargetData.IsInMaxDistance(casterPos, u.transform.position));
        
        // 필터 적용
        SelectActiveUnit(targets); // 활성화된 Unit만 선택
        targets = ApplyTeamFilter(lineTargetData, caster, targets);
        targets = ApplyConditionFilter(lineTargetData, targets);
        targets = ApplySelect(lineTargetData, targets);
        
        UnitController nearestTarget = FindNearestTarget(casterPos, targets);
        
        scanResult.Targets = targets;
        scanResult.PrimaryTarget = nearestTarget;
        
        return scanResult;
    }
}
