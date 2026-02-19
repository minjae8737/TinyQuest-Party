using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class SkillTargetScanner : ScriptableObject
{
    protected const float MAX_SCAN_RADIUS = 20f;

    [Header("=== Scan Settings ===")] 
    [SerializeField] protected ContactFilter2D contactFilter;
    
    protected Collider2D[] enemies = new Collider2D[30];

    public abstract SkillScanResult Scan(UnitController caster, SkillTargetData targetData);

    protected void GetUnitController(int count, List<UnitController> targets)
    {
        for (int i = 0; i < count; i++)
        {
            if (enemies[i].TryGetComponent<UnitController>(out var controller))
            {
                targets.Add(controller);
            }
        }
    }
    
    protected Collider2D FindNearestEnemy(Vector2 casterPos, int count)
    {
        float minDistSqr = MAX_SCAN_RADIUS * MAX_SCAN_RADIUS;

        Collider2D nearestEnemy = null;

        for (int i = 0; i < count; i++)
        {
            Collider2D enemy = enemies[i];
            if (enemy == null) continue;

            Vector2 diff = (Vector2)enemy.transform.position - casterPos;
            float distSqr = diff.sqrMagnitude;

            if (distSqr < minDistSqr)
            {
                minDistSqr = distSqr;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }
    
    protected UnitController FindNearestTarget(Vector2 casterPos, List<UnitController> targets)
    {
        float minDistSqr = MAX_SCAN_RADIUS * MAX_SCAN_RADIUS;
        UnitController nearestTarget = null;

        for (int i = 0; i < targets.Count; i++)
        {
            UnitController target = targets[i];
            if (target == null) continue;

            Vector2 diff = (Vector2)target.transform.position - casterPos;
            float distSqr = diff.sqrMagnitude;

            if (distSqr < minDistSqr)
            {
                minDistSqr = distSqr;
                nearestTarget = target;
            }
        }

        return nearestTarget;
    }

    protected List<UnitController> ApplyTeamFilter(SkillTargetData targetData, UnitController caster, List<UnitController> targets)
    {
        //TODO LINQ 사용을 줄여 메모리 최적화 필요
        switch (targetData.TargetLayer)
        {
            case TargetLayerType.Enemy:
                return targets.FindAll(u => u.TeamType != caster.TeamType);
            
            case TargetLayerType.Ally:
                return targets.FindAll(u => u.TeamType == caster.TeamType);
            
            case TargetLayerType.Self:
                return targets.FindAll(u => u == caster);
            
            case TargetLayerType.All:
            default:
                return targets;
        }
    }
    
    protected List<UnitController> ApplyConditionFilter(SkillTargetData targetData, List<UnitController> targets)
    {
        //TODO LINQ 사용을 줄여 메모리 최적화 필요
        switch (targetData.SortType)
        {
            case TargetSortType.LowHp:
                return targets.OrderBy(u => u.CurrentHp).ToList();
            
            case TargetSortType.LowHpPercent:
                return targets.OrderBy(u => u.HpPercent).ToList();
                
            case TargetSortType.HighHp:
                return targets.OrderByDescending(u => u.CurrentHp).ToList();
            
            case TargetSortType.HighHpPercent:
                return targets.OrderByDescending(u => u.HpPercent).ToList();
            
            case TargetSortType.None:
            default:
                return targets;
        }
    }
    
    protected List<UnitController> ApplySelect(SkillTargetData targetData, List<UnitController> targets)
    {
        //TODO LINQ 사용을 줄여 메모리 최적화 필요
        int maxTargetCount = targetData.MaxTargetCount;
        // maxTargetCount가 0일 경우는 제한없음.
        return maxTargetCount > 0 ? targets.Take(maxTargetCount).ToList() : targets; 
    }
}