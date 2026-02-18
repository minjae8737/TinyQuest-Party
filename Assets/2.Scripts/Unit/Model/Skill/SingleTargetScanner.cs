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

        // Overlap 스캔
        int count = Physics2D.OverlapCircle(casterPos, targetData.GetSkillDistance(), contactFilter, enemies);
        
        // enemies 에서 UnitController 추출
        GetUnitController(count, targets);
        
        // 필터 적용
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