using System;
using System.Collections.Generic;
using UnityEngine;

public class TargetScanner : MonoBehaviour
{
    private const float MAX_SCAN_RADIUS = 20f;

    [SerializeField] private LayerMask enemyLayer;
    private Collider2D[] enemies = new Collider2D[30];
    private ContactFilter2D filter;
    private Vector2 forward;

    private List<UnitController> targets;

    void Awake()
    {
        // ContactFilter2D 설정
        filter = new ContactFilter2D();
        filter.SetLayerMask(enemyLayer);
        filter.useLayerMask = true;
        filter.useTriggers = false;

        targets = new List<UnitController>();
    }

    public List<UnitController> Scan(Skill skill, bool isForwardLeft)
    {
        targets.Clear();
        forward = isForwardLeft ? Vector2.left : Vector2.right;

        // 캐릭터 주변 MAX_SCAN_RADIUS 안 대상 타깃
        int count = Physics2D.OverlapCircle(transform.position, MAX_SCAN_RADIUS, filter, enemies);
        Debug.Log("Targets Count : " + count);
        switch (skill.SkillTargetType)
        {
            case SkillTargetType.Single:
                SelectNearestTarget(count, forward, skill);
                break;
            case SkillTargetType.Circle:
                SelectCircleTarget(count, forward, skill);
                break;
            case SkillTargetType.Cone:
                SelectConeTarget(count, forward, skill);
                break;
            case SkillTargetType.Line:
                SelectLineTarget(count, forward, skill);
                break;
        }

        return targets;
    }

    private Collider2D FindNearestEnemy(int count)
    {
        // sqrMagnitude 로 비교하기위한 제곱
        float minDistSqr = MAX_SCAN_RADIUS * MAX_SCAN_RADIUS;
        Collider2D nearestEnemy = null;

        for (int i = 0; i < count; i++)
        {
            Collider2D enemy = enemies[i];
            if (enemy == null) continue;

            Vector2 diff = enemy.transform.position - transform.position;
            float distSqr = diff.sqrMagnitude;

            if (distSqr < minDistSqr)
            {
                minDistSqr = distSqr;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    public void SelectNearestTarget(int count, Vector2 forward, Skill skill)
    {
        Vector2 myPos = transform.position;
        SingleTargetData skillTargetData = (SingleTargetData)skill.TargetData;
        debugSkill = skill;
        Collider2D nearestEnemy = FindNearestEnemy(count);
        if (!skillTargetData.IsInRange(myPos, nearestEnemy.transform.position, forward)) return;

        if (nearestEnemy != null && nearestEnemy.TryGetComponent<UnitController>(out var controller))
        {
            targets.Add(controller);
        }
    }

    public void SelectConeTarget(int count, Vector2 forward, Skill skill)
    {
        Vector2 myPos = transform.position;
        ConeTargetData skillTargetData = (ConeTargetData)skill.TargetData;
        debugSkill = skill;
        
        for (int i = 0; i < count; i++)
        {
            Collider2D enemy = enemies[i];
            if (enemy == null) continue;

            if (skillTargetData.IsInRange(myPos, enemy.transform.position, forward))
            {
                if (enemy.TryGetComponent<UnitController>(out var controller))
                {
                    targets.Add(controller);
                }
            }
        }
    }

    public void SelectCircleTarget(int count, Vector2 forward, Skill skill)
    {
        Vector2 myPos = transform.position;
        CircleTargetData skillTargetData = (CircleTargetData)skill.TargetData;
        debugSkill = skill;
        // 가장 가까운 대상 탐색
        Collider2D nearestEnemy = FindNearestEnemy(count);
        if (nearestEnemy == null) return;

        Vector2 targetPos = nearestEnemy.transform.position;
        if (!skillTargetData.IsInMaxDistance(myPos, nearestEnemy.transform.position)) return;
        debugTargetPos = targetPos; // Gizmo용 

        // target 중심으로 재탐색
        int enemyCounts = Physics2D.OverlapCircle(targetPos, MAX_SCAN_RADIUS, filter, enemies);

        for (int i = 0; i < enemyCounts; i++)
        {
            Collider2D enemy = enemies[i];

            if (!skillTargetData.IsInMaxRange(targetPos, enemy.transform.position)) continue;

            if (enemy.TryGetComponent<UnitController>(out var controller))
            {
                targets.Add(controller);
            }
        }
    }

    public void SelectLineTarget(int count, Vector2 forward, Skill skill)
    {
        Vector2 myPos = transform.position;
        LineTargetData skillTargetData = (LineTargetData)skill.TargetData;
        debugSkill = skill;

        // 가장 가까운 대상 탐색
        Collider2D nearestEnemy = FindNearestEnemy(count);
        if (nearestEnemy == null) return;
        
        Vector2 targetPos = nearestEnemy.transform.position;
        if (!skillTargetData.IsInMaxDistance(myPos, targetPos)) return;
        debugTargetPos = targetPos; // Gizmo용 

        Vector2 boxArea = skillTargetData.GetBoxArea();
        float angle = Mathf.Abs(90 - skillTargetData.GetAngle(myPos, targetPos, forward));

        // target 중심으로 재탐색
        Vector2 toTargetDir = (targetPos - forward).normalized;
        Vector2 point = targetPos + toTargetDir * (boxArea.y * 0.5f);
        Physics2D.OverlapBox(point, boxArea, angle, filter, enemies);
    }

    # region Gizmo
    /// <summary>
    /// Color.green - 나의 공격 범위
    /// Color.red - 스킬 피격 범위
    /// </summary>
    private Skill debugSkill;
    private Vector2 debugTargetPos;
    private Vector2 debugForward => forward;

    void OnDrawGizmosSelected()
    {
        if (debugSkill == null) return;
        if (debugSkill.TargetData == null) return;

        DrawTargetGizmos(debugSkill.TargetData);
    }

    void DrawTargetGizmos(SkillTargetData targetData)
    {
        // FIXME 나중에 Collider 오프셋을 계산한 myPos 로 변경
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
        Gizmos.color = Color.red;
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(myPos, data.GetBoxArea());
    }

    #endregion
}