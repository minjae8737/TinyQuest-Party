using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Scan/SelfCircle")]
public class SelfCircleTargetScanner : SkillTargetScanner
{
    public override List<UnitController> Scan(UnitController caster, SkillData skillData)
    {
        CircleTargetData targetData = (CircleTargetData)skillData.TargetData;
        Vector2 casterPos = caster.transform.position;
        List<UnitController> targets = new List<UnitController>();

        // target 중심으로 재탐색
        int enemyCounts = Physics2D.OverlapCircle(casterPos, targetData.Radius, contactFilter, enemies);

        Debug.Log("SelfCircleTarget enemyCounts : " + enemyCounts);
        
        for (int i = 0; i < enemyCounts; i++)
        {
            Collider2D enemy = enemies[i];
            
            if (enemy.TryGetComponent<UnitController>(out var controller))
            {
                targets.Add(controller);
            }
        }

        return targets;
    }
}
