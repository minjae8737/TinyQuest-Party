using System.Collections.Generic;
using UnityEngine;

public class HealSkillData : SkillData
{
    [Header("=== HealAmount Info ===")]
    [Min(0)] public float HealAmount;
    
    public override void Use(UnitController caster, List<UnitController> targets)
    {
    }
}
