using System;
using System.Collections.Generic;

public class DamageSkillData : SkillData
{
    public float Damage;

    protected void ApplyDamage(UnitController caster, List<UnitController> targets)
    {
        int damage = (int)Math.Round(caster.Model.Stat.Atk + Damage);

        foreach (UnitController target in targets)
        {
            target.Model.TakeDamage(damage);
        }
    }
}
