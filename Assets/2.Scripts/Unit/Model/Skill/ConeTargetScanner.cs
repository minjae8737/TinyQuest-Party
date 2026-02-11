using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Scan/Cone")]
public class ConeTargetScanner : SkillTargetScanner
{
    public override List<UnitController> Scan(UnitController caster, SkillData skillData)
    {
        ConeTargetData targetData = (ConeTargetData)skillData.TargetData;
        Vector2 casterPos = caster.transform.position;
        Vector2 forward = caster.Forward;
        List<UnitController> targets = new List<UnitController>();
        
        int count = Physics2D.OverlapCircle(casterPos, targetData.GetSkillDistance(), contactFilter, enemies);
        targetPos = casterPos;
        
        for (int i = 0; i < count; i++)
        {
            Collider2D enemy = enemies[i];
            if (enemy == null) continue;

            if (targetData.IsInRange(casterPos, enemy.transform.position, forward))
            {
                if (enemy.TryGetComponent<UnitController>(out var controller))
                {
                    targets.Add(controller);
                }
            }
        }

        return targets;
    }
}
