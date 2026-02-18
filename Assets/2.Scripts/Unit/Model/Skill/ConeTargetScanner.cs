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

        // Overlap 스캔
        int count = Physics2D.OverlapCircle(casterPos, coneTargetData.GetSkillDistance(), contactFilter, enemies);
        GetUnitController(count, targets);

        // 필터 적용
        targets = ApplyTeamFilter(coneTargetData, caster, targets);
        targets = ApplyConditionFilter(coneTargetData, targets);
        targets = ApplySelect(coneTargetData, targets);

        List<UnitController> filteredTargets = new List<UnitController>();
        foreach (UnitController target in targets)
        {
            if (coneTargetData.IsInRange(casterPos, target.transform.position, forward))
            {
                filteredTargets.Add(target);
            }
        }

        scanResult.Targets = filteredTargets;
        scanResult.PrimaryTarget = caster;

        return scanResult;
    }
}