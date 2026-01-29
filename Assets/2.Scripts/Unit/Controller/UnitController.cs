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

    public void DoAttack(int skillIdx, Vector2 curPos, float curTime)
    {
        if (!canMove || model.IsDeath) return;

        Skill skill = model.GetSkill(skillIdx);
        if (skill == null)
        {
            Debug.LogError("Skill is Null : " + transform.name);
            return;
        }

        // search target
        targets = scanner.Scan(skill, isForwardLeft);

        if (targets.Count <= 0) return; // TODO 타겟이 없다면 취소 targets 검증방법 수정
        if (skill == null || !skill.CanUse(curTime)) return; // 쿨타임중이라면 취소

        skill.Use(curTime);
        view.PlayAttack(skillIdx);
        LookAt(curPos, targets[0].transform.position);
        StartCoroutine(UseSkill(skill));
    }

    private IEnumerator UseSkill(Skill skill)
    {
        canMove = false;
        yield return new WaitForSeconds(skill.CastTime); // 선딜
        
        // 공격 데미지 주는 시점 (공격력 + 스킬 데미지)
        int damage = (int)Math.Round(model.Stat.Atk + skill.Damage);
        foreach (UnitController target in targets)
        {
            target.model.TakeDamage(damage);
        }

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