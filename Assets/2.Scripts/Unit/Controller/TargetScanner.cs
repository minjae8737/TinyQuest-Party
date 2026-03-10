using System;
using System.Collections.Generic;
using UnityEngine;

public class TargetScanner : MonoBehaviour
{
    [SerializeField] private TargetLayerType enemyLayer;
    
    private const float MAX_SCAN_RADIUS = 20f;

    public UnitController FindNearestEnemy(UnitController caster)
    {
        Vector2 casterPos = caster.transform.position;
        List<UnitController> targets = new List<UnitController>();
        targets = new(UnitManager.Instance.Units);
        
        // 필터 적용
        targets = ApplyTeamFilter(caster, targets);

        UnitController nearestTarget = FindNearestEnemy(casterPos, targets);
        targets.Clear();
        targets.Add(nearestTarget);

        return nearestTarget;
    }

    private UnitController FindNearestEnemy(Vector2 casterPos, List<UnitController> targets)
    {
        // sqrMagnitude 로 비교하기위한 제곱
        float minDistSqr = MAX_SCAN_RADIUS * MAX_SCAN_RADIUS;
        UnitController nearestEnemy = null;

        for (int i = 0; i < targets.Count; i++)
        {
            UnitController target = targets[i];
            if (target == null) continue;

            Vector2 diff = (Vector2)target.transform.position - casterPos;
            float distSqr = diff.sqrMagnitude;

            if (distSqr < minDistSqr)
            {
                minDistSqr = distSqr;
                nearestEnemy = target;
            }
        }

        return nearestEnemy;
    }
    
    private List<UnitController> ApplyTeamFilter(UnitController caster, List<UnitController> targets)
    {
        //TODO LINQ 사용을 줄여 메모리 최적화 필요
        switch (enemyLayer)
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

    // public void SelectNearestTarget(int count, Skill skill, Vector2 casterPos, Vector2 forward)
    // {
    //     SingleTargetData skillTargetData = (SingleTargetData)skill.Data.TargetData;
    //     // debugSkill = skill;
    //     
    //     Collider2D nearestEnemy = FindNearestEnemy(count);
    //     if (nearestEnemy == null || !skillTargetData.IsInRange(casterPos, nearestEnemy.transform.position, forward)) return;
    //
    //     if (nearestEnemy != null && nearestEnemy.TryGetComponent<UnitController>(out var controller))
    //     {
    //         targets.Add(controller);
    //     }
    // }

    // public void SelectConeTarget(int count, Skill skill, Vector2 casterPos, Vector2 forward)
    // {
    //     ConeTargetData skillTargetData = (ConeTargetData)skill.Data.TargetData;
    //     // debugSkill = skill;
    //
    //     for (int i = 0; i < count; i++)
    //     {
    //         Collider2D enemy = enemies[i];
    //         if (enemy == null) continue;
    //
    //         if (skillTargetData.IsInRange(casterPos, enemy.transform.position, forward))
    //         {
    //             if (enemy.TryGetComponent<UnitController>(out var controller))
    //             {
    //                 targets.Add(controller);
    //             }
    //         }
    //     }
    // }
    //
    // public void SelectCircleTarget(int count, Skill skill, Vector2 casterPos, Vector2 forward)
    // {
    //     CircleTargetData skillTargetData = (CircleTargetData)skill.Data.TargetData;
    //     // debugSkill = skill;
    //
    //     // 가장 가까운 대상 탐색
    //     Collider2D nearestEnemy = FindNearestEnemy(count);
    //     if (nearestEnemy == null) return;
    //
    //     Vector2 targetPos = nearestEnemy.transform.position;
    //     if (!skillTargetData.IsInMaxDistance(casterPos, targetPos)) return;
    //     
    //     skillTargetPos = targetPos; // Gizmo용 
    //
    //     // target 중심으로 재탐색
    //     int enemyCounts = Physics2D.OverlapCircle(targetPos, MAX_SCAN_RADIUS, filter, enemies);
    //
    //     for (int i = 0; i < enemyCounts; i++)
    //     {
    //         Collider2D enemy = enemies[i];
    //
    //         if (!skillTargetData.IsInMaxRange(targetPos, enemy.transform.position)) continue;
    //
    //         if (enemy.TryGetComponent<UnitController>(out var controller))
    //         {
    //             targets.Add(controller);
    //         }
    //     }
    // }
    //
    // public void SelectLineTarget(int count, Skill skill, Vector2 casterPos, Vector2 forward)
    // {
    //     LineTargetData skillTargetData = (LineTargetData)skill.Data.TargetData;
    //     // debugSkill = skill;
    //
    //     // 가장 가까운 대상 탐색
    //     Collider2D nearestEnemy = FindNearestEnemy(count);
    //     if (nearestEnemy == null) return;
    //
    //     Vector2 targetPos = nearestEnemy.transform.position;
    //     if (!skillTargetData.IsInMaxDistance(casterPos, targetPos)) return;
    //     skillTargetPos = targetPos; // Gizmo용 
    //
    //     Vector2 boxArea = skillTargetData.GetBoxArea();
    //     float angle = skillTargetData.GetAngle(casterPos, targetPos, forward);
    //
    //     // 스킬 범위 중심으로 재탐색
    //     Vector2 toTargetDir = (targetPos - casterPos).normalized;
    //     Vector2 point = casterPos + toTargetDir * (boxArea.x * 0.5f);
    //
    //     int enemyCounts = Physics2D.OverlapBox(point, boxArea, angle, filter, enemies);
    //
    //     for (int i = 0; i < enemyCounts; i++)
    //     {
    //         Collider2D enemy = enemies[i];
    //
    //         if (enemy.TryGetComponent<UnitController>(out var controller))
    //         {
    //             targets.Add(controller);
    //         }
    //     }
    // }
 /**
    # region Gizmo

    /// <summary>
    /// Color.green - 나의 공격 범위
    /// Color.red - 스킬 피격 범위
    /// </summary>
    private Skill debugSkill;

    private Vector2 debugTargetPos => skillTargetPos;
    private Vector2 debugForward => forward;

    void OnDrawGizmosSelected()
    {
        if (debugSkill == null) return;
        if (debugSkill.Data.TargetData == null) return;

        DrawTargetGizmos(debugSkill.Data.TargetData);
    }

    void DrawTargetGizmos(SkillTargetData targetData)
    {
        // TODO 나중에 Collider 오프셋을 계산한 myPos 로 변경
        Vector2 myPos = transform.position;

        switch (targetData.TargetType)
        {
            case SkillTargetType.Single:
                DrawSingleGizmos(myPos, (SingleTargetData)targetData);
                break;

            case SkillTargetType.Circle:
                DrawCircleGizmos(myPos, (CircleTargetData)targetData);
                break;

            case SkillTargetType.Cone:
                DrawConeGizmos(myPos, (ConeTargetData)targetData);
                break;

            case SkillTargetType.Line:
                DrawLineGizmos(myPos, (LineTargetData)targetData);
                break;
        }
    }

    void DrawSingleGizmos(Vector2 myPos, SingleTargetData data)
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(myPos, data.MaxDistance); // 사거리
    }

    void DrawCircleGizmos(Vector2 myPos, CircleTargetData data)
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(myPos, data.MaxDistance); // 사거리
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(debugTargetPos, data.Radius); // 타겟 주변 원형 범위
    }

    void DrawConeGizmos(Vector2 myPos, ConeTargetData data)
    {
        float angle = data.Angle * 0.5f; // 각도
        float length = data.Length; // 길이
        float angleInRadians = angle * Mathf.Deg2Rad;

        Vector2 dir1 = new Vector2(Mathf.Cos(angleInRadians) * debugForward.x, Mathf.Sin(angleInRadians)) * length;
        Vector2 dir2 = new Vector2(Mathf.Cos(angleInRadians) * debugForward.x, -Mathf.Sin(angleInRadians)) * length;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(myPos, myPos + dir1); // 윗각 
        Gizmos.DrawLine(myPos, myPos + dir2); // 밑각
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(myPos, length); // 원뿔의 곡선표현
    }

    void DrawLineGizmos(Vector2 myPos, LineTargetData data)
    {
        Vector2 boxArea = data.GetBoxArea();
        Vector2 halfSize = boxArea * 0.5f;

        Vector2 toTargetDir = (debugTargetPos - myPos).normalized;
        Vector2 point = myPos + toTargetDir * (boxArea.x * 0.5f);

        // 박스 꼭짓점
        Vector2[] vertexs = new Vector2[4]
        {
            new Vector2(-halfSize.x, -halfSize.y),
            new Vector2(halfSize.x, -halfSize.y),
            new Vector2(halfSize.x, halfSize.y),
            new Vector2(-halfSize.x, halfSize.y)
        };

        float angle = data.GetAngle(myPos, debugTargetPos, forward);
        float rad = angle * Mathf.Deg2Rad; // 라디안으로 변환
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        // 좌표 회전
        for (int i = 0; i < vertexs.Length; i++)
        {
            Vector2 vertex = vertexs[i];
            vertexs[i] = new Vector2(
                vertex.x * cos - vertex.y * sin, // 회전 변환
                vertex.x * sin + vertex.y * cos // 회전 변환
            ) + point;
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(myPos, boxArea.x); // 사거리

        // 스킬 범위
        Gizmos.color = Color.red;
        Gizmos.DrawLine(vertexs[0], vertexs[1]);
        Gizmos.DrawLine(vertexs[1], vertexs[2]);
        Gizmos.DrawLine(vertexs[2], vertexs[3]);
        Gizmos.DrawLine(vertexs[3], vertexs[0]);
    }

    #endregion
    */
}