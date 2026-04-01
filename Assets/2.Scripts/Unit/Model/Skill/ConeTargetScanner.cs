using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Scan/Cone")]
public class ConeTargetScanner : SkillTargetScanner
{
    public override SkillScanResult Scan(UnitController caster, SkillTargetData targetData)
    {
        SkillScanResult scanResult = new SkillScanResult();
        ConeTargetData coneTargetData = (ConeTargetData)targetData;
        Vector2 casterPos = caster.transform.position;
        Vector2 forward = caster.Forward;
        List<UnitController> targets = new List<UnitController>();

        targets = new(UnitManager.Instance.Units);

        targets.RemoveAll(u => !coneTargetData.IsInRange(casterPos, u.transform.position, forward));
        
        // 필터 적용
        SelectActiveUnit(targets); // 활성화된 Unit만 선택
        targets = ApplyTeamFilter(coneTargetData, caster, targets);
        targets = ApplyConditionFilter(coneTargetData, targets);
        targets = ApplySelect(coneTargetData, targets);

        scanResult.Targets = targets;
        scanResult.PrimaryTarget = caster;

        return scanResult;
    }
}