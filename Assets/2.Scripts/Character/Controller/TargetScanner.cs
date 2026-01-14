using System.Collections.Generic;
using UnityEngine;

public class TargetScanner : MonoBehaviour
{
    private const float MAX_SCAN_RADIUS = 20f;

    [SerializeField] private LayerMask enemyLayer;
    private Collider2D[] enemies = new Collider2D[30];
    private ContactFilter2D filter;

    private List<CharacterController> targets;

    void Awake()
    {
        // ContactFilter2D 설정
        filter = new ContactFilter2D();
        filter.SetLayerMask(enemyLayer);
        filter.useLayerMask = true;
        filter.useTriggers = false;

        targets = new List<CharacterController>();
    }

    public List<CharacterController> Scan(Skill skill, bool isForwardLeft)
    {
        targets.Clear();
        Vector2 forward = isForwardLeft ? Vector2.left : Vector2.right;

        // 캐릭터 주변 MAX_SCAN_RADIUS 안 대상 타깃
        int count = Physics2D.OverlapCircle(transform.position, MAX_SCAN_RADIUS, filter, enemies);
        Debug.Log("Targets Count : " + count);
        switch (skill.SkillTargetType)
        {
            case SkillTargetType.Single:
                SelectNearestTarget(count);
                break;
            case SkillTargetType.Circle:
                break;
            case SkillTargetType.Cone:
                SelectConeTarget(count, forward, skill);
                break;
            case SkillTargetType.Line:
                break;
        }

        return targets;
    }

    public bool IsInAngle(Collider2D target, Vector2 forward, float angle)
    {
        Vector2 curPos = transform.position;
        Vector2 targetPos = target.transform.position;
        Vector2 toTarget = targetPos - curPos;

        return Vector2.Angle(forward, toTarget) <= angle * 0.5f; // forward 방향 기준 한쪽 각도
    }

    public bool IsInDist(Vector2 pos, Vector2 target, float targetDist)
    {
        Vector2 diff = target - pos;
        return diff.sqrMagnitude <= targetDist * targetDist; // magnitude 보다 가벼움
    }

    public void SelectNearestTarget(int count)
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

        if (nearestEnemy != null && nearestEnemy.TryGetComponent<CharacterController>(out var controller))
        {
            targets.Add(controller);
        }
    }

    public void SelectConeTarget(int count, Vector2 forward, Skill skill)
    {
        for (int i = 0; i < count; i++)
        {
            Collider2D enemy = enemies[i];
            if (enemy == null) continue;
            
            if (!IsInDist(transform.position, enemy.transform.position, skill.Range)) continue;
            
            if (enemy != null && IsInAngle(enemy, forward, skill.Angle))
            {
                if (enemy.TryGetComponent<CharacterController>(out var controller))
                {
                    targets.Add(controller);
                }
            }
        }
    }

    public void SelectCircleTarget(int count, Vector2 forward, Skill skill)
    {
        
    }

    public void SelecLineTarget(int count, Vector2 forward, Skill skill)
    {
        
    }
}