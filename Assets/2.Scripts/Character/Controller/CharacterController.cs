using System;
using System.Collections;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private Character model;
    public CharacterView view;

    private bool preIsLeft;
    private bool canMove = true;

    private void Awake()
    {
        // FIXME 임시값 초기화
        model.Speed = 5;
    }

    public void DoMove(Vector2 targetPos)
    {
        if (!canMove) return;
        
        // Move
        Vector2 curPos = transform.position;
        transform.position = Vector2.MoveTowards(curPos, targetPos, model.Speed * Time.deltaTime);
        // FIXME   Anim Speed 나중에 로직수정?
        float speed = Vector2.Distance(targetPos, curPos) > 0.001f ? 1 : 0;
        view.SetSpeed(speed);
        
        // Flip
        bool isLeft = curPos.x > targetPos.x;
        // FIXME curPos.x == targetPos.x  float값이니 근사값이면 같은거로 할수있게 
        if (curPos.x == targetPos.x) isLeft = preIsLeft;
        view.SetFlipX(isLeft);
        preIsLeft = isLeft;

        // Character Order
        view.SetOrderInLayer((int)(-transform.position.y * 100));
    }

    public void DoAttack(int skillIdx, Vector2 curPos, Vector2 targetPos, float curTime)
    {
        if (!canMove) return;
        
        Skill skill = model.GetSkill(skillIdx);
        if (skill == null || !skill.CanUse(curPos, targetPos, curTime)) return;

        skill.Use(curTime);
        view.AttackAnim(skillIdx);
        StartCoroutine(UseSkill(skill));
    }

    private IEnumerator UseSkill(Skill skill)
    {
        canMove = false;
        yield return new WaitForSeconds(skill.CastTime);  // 선딜
        
        // 공격 데미지 주는 시점
        
        yield return new WaitForSeconds(skill.RecoveryTime); // 후딜
        canMove = true;
    } 
}