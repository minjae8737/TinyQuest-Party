using System;
using System.Collections;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Unit model;
    [SerializeField] private UnitView view;
    [SerializeField] private TargetScanner scanner;
    [SerializeField] private CapsuleCollider2D col;

    [Header("Movement")]
    private bool isForwardLeft;
    private bool canMove = true;
    private Vector2? nextPos;
    
    #region Property
    
    public Unit Model => model;
    public Vector2 Forward => isForwardLeft ? Vector2.left : Vector2.right;
    
    #endregion
    
    #region Init
    
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
        col.enabled = true;
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
    
    #endregion

    #region Move

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
        skillIdx = Math.Max(skillIdx, 0);
        Skill skill = model.GetSkill(skillIdx);

        UnitController nearestEnemy = scanner.FindNearestEnemy();
        if (nearestEnemy == null) return false;

        float skillRange = skill.Data.TargetData.GetSkillDistance() * 0.9f; // 0.9f -> 여유롭게 스킬범위 안으로 진입
        Vector2 dir = (transform.position - nearestEnemy.transform.position).normalized;

        nextPos = nearestEnemy.transform.position + (Vector3)(dir * skillRange);
        return true;
    }

    private void LookAt(Vector2 curPos, Vector2 nextPos)
    {
        bool isLeft = curPos.x > nextPos.x;
        
        if (Mathf.Abs(curPos.x - nextPos.x) < 0.001f) isLeft = isForwardLeft;
        view.SetFlipX(isLeft);
        
        isForwardLeft = isLeft;
    }
    
    #endregion

    #region Combat
    
    public bool CanAttack(int skillIdx)
    {
        if (!canMove || model.IsDeath) return false;

        Skill skill = model.GetSkill(skillIdx);

        return skill.CanCast(this); 
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
        
        view.PlayAttack(skillIdx);
        LookAt(curPos, skill.TargetPos);
        StartCoroutine(UseSkill(skill));
    }

    private IEnumerator UseSkill(Skill skill)
    {
        canMove = false;
        yield return new WaitForSeconds(skill.Data.CastTime); // 선딜
        
        skill.Use(this);  

        yield return new WaitForSeconds(skill.Data.RecoveryTime); // 후딜
        canMove = true;
    }
    
    #endregion

    #region Event Handler
    
    public void OnHpChanged(int maxHp, int hp)
    {
        if (model.IsDeath) return;
        
        view.RefreshHp(maxHp, hp);
        if (hp <= 0)
        {
            col.enabled = false;
            view.PlayDeath();
        }
    }

    public void HandleDeathFinished()
    {
        UnitManager.Instance.Despawn(this);
    }
    
    #endregion
}
