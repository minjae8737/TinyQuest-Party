using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class SkillData : ScriptableObject
{
    public SkillType Type;
    public SkillTargetType TargetType;
    public SkillTargetData TargetData;
    public ProjectileStartType StartType;
    public SkillDeliveryType DeliveryType;
    
    public AnimationClip effectClip;
    
    public float CastTime; // 선딜
    public float RecoveryTime; // 후딜
    public float Cooldown; // 재사용 대기

    public virtual void Use(UnitController caster, List<UnitController> targets) { }
}
