using System.Collections.Generic;
using UnityEngine;

public abstract class SkillTargetScanner : ScriptableObject
{
    protected const float MAX_SCAN_RADIUS = 20f;
    
    [Header("=== Scan Settings ===")]
    [SerializeField] protected ContactFilter2D contactFilter;
    [SerializeField] private LayerMask enemyLayer;
    
    protected Collider2D[] enemies = new Collider2D[30];
    
    protected Vector2 targetPos;
    public Vector2 TargetPos => targetPos;
    
    public abstract List<UnitController> Scan(UnitController caster, SkillData skillData);

    public Collider2D FindNearestEnemy(Vector2 casterPos, int count)
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
}