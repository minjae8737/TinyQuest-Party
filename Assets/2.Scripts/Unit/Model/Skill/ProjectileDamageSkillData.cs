using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage_(UnitName)_(SkillSlot)", menuName = "Skill/SkillData/DamageSkill/Projectile")]
public class ProjectileDamageSkillData : DamageSkillData
{
    public float Speed;
    
    public override void Use(UnitController caster, List<UnitController> targets)
    {
        ApplyDamage(caster, targets);
    }
}