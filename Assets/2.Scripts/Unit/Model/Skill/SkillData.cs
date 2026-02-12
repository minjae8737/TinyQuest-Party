using System.Collections.Generic;
using UnityEngine;

public class SkillData : ScriptableObject
{
    [Header("=== Basic Info ===")]
    public SkillType Type;
    public SkillTargetType TargetType;
    public SkillDeliveryType DeliveryType;
    
    [Space(5)]
    [SerializeReference] public SkillTargetData TargetData;

    [Header("=== Timing ===")]
    [Tooltip("스킬 시전 전 대기 시간")] [Min(0)] public float CastTime; // 선딜
    [Tooltip("스킬 시전 후 대기 시간")] [Min(0)] public float RecoveryTime; // 후딜
    [Tooltip("다음 스킬 시전까지 대기 시간")] [Min(0)] public float Cooldown; // 재사용 대기

    [Header("=== Visual ===")]
    public AnimationClip EffectClip;

    public virtual void Use(UnitController caster, List<UnitController> targets) { }
}
