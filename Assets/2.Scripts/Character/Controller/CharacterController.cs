using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private Character model;
    public CharacterView view;

    private bool isForwardLeft;
    private bool canMove = true;

    [SerializeField] private TargetScanner scanner;
    [SerializeField] private List<CharacterController> targets;

    private void Awake()
    {
        // FIXME 임시값 초기화
        model.Speed = 5;
    }

    private void OnEnable()
    {
        model.OnHpChanged += OnHpChanged;
    }

    private void OnDisable()
    {
        model.OnHpChanged -= OnHpChanged;
    }

    public void DoMove(Vector2 targetPos)
    {
        if (!canMove) return;

        // Move
        Vector2 curPos = transform.position;
        transform.position = Vector2.MoveTowards(curPos, targetPos, model.Speed * Time.deltaTime);
        float speed = (targetPos - curPos).sqrMagnitude > 0.001f ? 1 : 0;
        view.SetSpeed(speed);

        // Flip
        bool isLeft = curPos.x > targetPos.x;
        // FIXME curPos.x == targetPos.x  float값이니 근사값이면 같은거로 할수있게 
        if (curPos.x == targetPos.x) isLeft = isForwardLeft;
        view.SetFlipX(isLeft);
        isForwardLeft = isLeft;

        // Character Order
        view.SetOrderInLayer((int)(-transform.position.y * 100));
    }

    public void DoAttack(int skillIdx, Vector2 curPos, float curTime)
    {
        if (!canMove) return;

        Skill skill = model.GetSkill(skillIdx);

        // search target
        targets = scanner.Scan(skill, isForwardLeft);

        if (targets.Count <= 0) return; // FIXME 타겟이 없다면 취소 targets 검증방법 수정
        if (skill == null || !skill.CanUse(curTime)) return; // 쿨타임중이라면 취소

        skill.Use(curTime);
        view.PlayAttack(skillIdx);
        StartCoroutine(UseSkill(skill));
    }

    private IEnumerator UseSkill(Skill skill)
    {
        canMove = false;
        yield return new WaitForSeconds(skill.CastTime); // 선딜

        // 공격 데미지 주는 시점
        int damage = (int)Math.Round(model.Atk + (model.Atk * skill.Damage));
        foreach (CharacterController target in targets)
        {
            target.model.TakeDamage(damage);
        }

        yield return new WaitForSeconds(skill.RecoveryTime); // 후딜
        canMove = true;
    }

    public void OnHpChanged(int hp)
    {
        if (hp <= 0) view.PlayDeath();
    }
}