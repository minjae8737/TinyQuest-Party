using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    [SerializeField] private Unit model;
    [SerializeField] private UnitView view;

    private bool isForwardLeft;
    private bool canMove = true;

    [SerializeField] private TargetScanner scanner;
    [SerializeField] private List<UnitController> targets;
    private Vector2? nextPos;

    public Unit Model => model;

    private void Start()
    {
        // 임시 초기화 위치
        Init();
    }

    public void Init(UnitSaveData saveData = null)
    {
        view.Init();
        model.Init(saveData);
        canMove = true;
    }

    private void OnEnable()
    {
        model.OnHpChanged += OnHpChanged;
        view.OnDeathFinished += HandleDeathFinished;
    }

    private void OnDisable()
    {
        model.OnHpChanged -= OnHpChanged;
        view.OnDeathFinished -= HandleDeathFinished;
    }

    private void Update()
    {
        if (nextPos.HasValue)
        {
            DoMove(nextPos.Value);
        }
    }
    
    public void SetNextPos(Vector2 pos)
    {
        nextPos = pos;
    }

    public void DoMove(Vector2 nextPos)
    {
        if (!canMove || model.IsDeath) return;

        // Move
        Vector2 curPos = transform.position;
        transform.position = Vector2.MoveTowards(curPos, nextPos, model.Stat.Speed * Time.deltaTime);
        float speed = (nextPos - curPos).sqrMagnitude > 0.001f ? 1 : 0;
        view.SetSpeed(speed);

        // Flip
        LookAt(curPos, nextPos);

        // Character Order
        view.SetOrderInLayer((int)(-transform.position.y * 100));
    }

    public bool TryGetNextPos(int skillIdx, out Vector2 nextPos)
    {
        nextPos = Vector2.zero;
        Skill skill = model.GetSkill(skillIdx);

        UnitController nearestEnemy = scanner.FindNearestEnemy();
        if (nearestEnemy == null) return false;

        float skillRange = skill.TargetData.GetSkillDistance() * 0.9f; // 0.9f -> 여유롭게 스킬범위 안으로 진입
        Vector2 dir = (transform.position - nearestEnemy.transform.position).normalized;

        nextPos = nearestEnemy.transform.position + (Vector3)(dir * skillRange);
        return true;
    }

    public bool CanAttack(int skillIdx)
    {
        if (!canMove || model.IsDeath) return false;

        Skill skill = model.GetSkill(skillIdx);

        // search target
        targets = scanner.Scan(skill, isForwardLeft);
        
        return targets.Count > 0;
    }

    public bool CanUseSkill(int skillIdx, float curTime)
    {
        Skill skill = model.GetSkill(skillIdx);
        if (skill == null) return false;
        
        return skill.CanUse(curTime);
    }

    public void DoAttack(int skillIdx, Vector2 curPos, float curTime)
    {
        Skill skill = model.GetSkill(skillIdx);
        
        skill.Use(curTime);
        view.PlayAttack(skillIdx);
        LookAt(curPos, targets[0].transform.position);
        StartCoroutine(UseSkill(skill));
    }

    private IEnumerator UseSkill(Skill skill)
    {
        canMove = false;
        yield return new WaitForSeconds(skill.CastTime); // 선딜

        if (skill.effectClip != null)
        {
            Vector2 targetPos = scanner.skillTargetPos;
            SkillEffect skillEffect = UnitManager.Instance.SpawnSkillEffect(targetPos);
            skillEffect.Play(skill.effectClip);
        }
        
        // 공격 데미지 주는 시점 (공격력 + 스킬 데미지)
        int damage = (int)Math.Round(model.Stat.Atk + skill.Damage);
        foreach (UnitController target in targets)
        {
            target.model.TakeDamage(damage);
        }

        targets = null;
        
        yield return new WaitForSeconds(skill.RecoveryTime); // 후딜
        canMove = true;
    }

    private void LookAt(Vector2 curPos, Vector2 nextPos)
    {
        bool isLeft = curPos.x > nextPos.x;
        
        if (Mathf.Abs(curPos.x - nextPos.x) < 0.001f) isLeft = isForwardLeft;
        view.SetFlipX(isLeft);
        
        isForwardLeft = isLeft;
    }

    public void OnHpChanged(int maxHp, int hp)
    {
        if (model.IsDeath) return;
        
        view.RefreshHp(maxHp, hp);
        if (hp <= 0) view.PlayDeath();
    }

    public void HandleDeathFinished()
    {
        UnitManager.Instance.Despawn(this);
    }
}