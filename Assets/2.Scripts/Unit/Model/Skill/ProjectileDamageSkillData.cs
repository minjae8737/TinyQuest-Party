using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage_(UnitName)_(SkillSlot)", menuName = "Skill/SkillData/DamageSkill/Projectile")]
public class ProjectileDamageSkillData : DamageSkillData
{
    [Header("=== Projectile Info ===")]
    [Min(0)] public float Speed;
    
    public override void Use(UnitController caster, List<UnitController> targets)
    {
        ApplyDamage(caster, targets);
        AudioManager.Instance.PlaySfx(Sfx);
    }
}