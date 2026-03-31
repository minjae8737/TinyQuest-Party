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
    public TeamType TeamType => Model.TeamType;
    public Vector2 Forward => isForwardLeft ? Vector2.left : Vector2.right;
    public int CurrentHp => model.Status.Hp;
    public int MaxHp => model.Status.MaxHp;
    public float HpPercent => (float)model.Status.Hp / model.Status.MaxHp * 100;
    
    #endregion

    public event Action<float> OnDamage;
    
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
        OnDamage += view.HandleDamage;
    }

    private void OnDisable()
    {
        model.OnHpChanged -= OnHpChanged;
        view.OnDeathFinished -= HandleDeathFinished;
        OnDamage -= view.HandleDamage;
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
        // view.SetOrderInLayer((int)(-transform.position.y * 100));
    }

    public bool TryGetNextPos(int skillIdx, out Vector2 nextPos)
    {
        nextPos = Vector2.zero;
        skillIdx = Math.Max(skillIdx, 0);
        Skill skill = model.GetSkill(skillIdx);

        UnitController nearestEnemy = scanner.FindNearestEnemy(this);
        if (nearestEnemy == null) return false;

        float skillRange = skill.Data.TargetData.GetSkillDistance() * 0.9f; // 0.9f -> 여유롭게 스킬범위 안으로 진입
        Vector2 dir = (transform.position - nearestEnemy.transform.position).normalized;
        if (Mathf.Abs(transform.position.x - nearestEnemy.transform.position.x) < skillRange) return false; // 이미 스킬범위 안에 있다면 움직이지 않음  

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
        if (skill == null) return false;

        return skill.CanCast(this); 
    }

    public bool CanUseSkill(int skillIdx, float curTime)
    {
        Skill skill = model.GetSkill(skillIdx);
        if (skill == null) return false;
        
        return skill.CanUse(curTime);
    }

    public bool DoAttack(int skillIdx, Vector2 curPos, float curTime)
    {
        if (!canMove || model.IsDeath) return false;
        
        Skill skill = model.GetSkill(skillIdx);
        
        if (!skill.CanUse(curTime)) return false;

        if (!skill.CanCast(this)) return false;

        UnitController target = skill.ScanResult.PrimaryTarget;
        if (!target) return false;
        
        view.PlayAttack(skillIdx);
        LookAt(curPos, skill.ScanResult.PrimaryTarget.transform.position);
        StartCoroutine(UseSkill(skill));
        
        return true;
    }

    private IEnumerator UseSkill(Skill skill)
    {
        canMove = false;
        yield return new WaitForSeconds(skill.Data.CastTime); // 선딜
        
        skill.Use(this);

        yield return new WaitForSeconds(skill.Data.RecoveryTime); // 후딜
        canMove = true;
    }
    
    public void TakeDamage(int damage)
    {
        float calDamage = model.TakeDamage(damage);
        
        if (model.TeamType == TeamType.Enemy)
        {
            OnDamage?.Invoke(calDamage);
        } 
    }

    public void TakeHeal(int healAmount)
    {
        model.TakeHeal(healAmount);
        
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
        if (model.TeamType == TeamType.Enemy)
        {
            StageManager.Instance.RequestStageReward(transform.position);
        }
        Despawn();
    }
    
    #endregion

    public void Despawn()
    {
        view.ReleaseHpbar();
        UnitManager.Instance.Despawn(this);
    }
}
