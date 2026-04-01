using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage_(UnitName)_(SkillSlot)",menuName = "Skill/SkillData/DamageSkill/Instant")]
public class InstantDamageSkillData : DamageSkillData
{
    public override void Use(UnitController caster, List<UnitController> targets)
    {
        ApplyDamage(caster, targets);
        AudioManager.Instance.PlaySfx(Sfx);
    }
}
