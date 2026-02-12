using System;
using System.Collections.Generic;
using UnityEngine;

public class DamageSkillData : SkillData
{
    [Header("=== Damage Info ===")]
    [Min(0)] public float Damage;

    protected void ApplyDamage(UnitController caster, List<UnitController> targets)
    {
        int damage = CalculateDamage(caster);

        foreach (UnitController target in targets)
        {
            target.Model.TakeDamage(damage);
        }
    }

    private int CalculateDamage(UnitController caster)
    {
        int damage = Mathf.RoundToInt(caster.Model.Stat.Atk + Damage);
        return damage;
    }
}
