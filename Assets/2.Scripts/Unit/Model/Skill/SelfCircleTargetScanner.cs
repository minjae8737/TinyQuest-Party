using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Scan/SelfCircle")]
public class SelfCircleTargetScanner : SkillTargetScanner
{
    public override SkillScanResult Scan(UnitController caster, SkillTargetData targetData)
    {
        SkillScanResult scanResult = new SkillScanResult();
        CircleTargetData circleTargetData = (CircleTargetData)targetData;
        Vector2 casterPos = caster.transform.position;
        List<UnitController> targets = new List<UnitController>();

        // Overlap 스캔
        int count = Physics2D.OverlapCircle(casterPos, circleTargetData.Radius, contactFilter, enemies);
        GetUnitController(count, targets);

        // 필터 적용
        targets = ApplyTeamFilter(circleTargetData, caster, targets);
        targets = ApplyConditionFilter(circleTargetData, targets);
        targets = ApplySelect(circleTargetData, targets);

        scanResult.Targets = targets;
        scanResult.PrimaryTarget = caster;

        return scanResult;
    }
}