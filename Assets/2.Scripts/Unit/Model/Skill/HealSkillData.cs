using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal_(UnitName)_(SkillSlot)",menuName = "Skill/SkillData/HealSkill")]
public class HealSkillData : SkillData
{
    [Header("=== HealAmount Info ===")]
    [Min(0)] public float HealAmount;
    
    public override void Use(UnitController caster, List<UnitController> targets)
    {
        // TODO Heal 타겟잡는것 수정 -> SelfCircleScanner에서 조금 더 변형시켜야함 -> HealEffect를 조절해야함. (public Vector2 TargetPos => targetPos; 를 List형으로 만들어서 전체힐로도 할 수 있어야할 듯)
        int healAmount = CalculateHealAmount(caster);
        int minHp = int.MaxValue;
        UnitController minHpTarget = null;
        
        foreach (UnitController target in targets)
        {
            if (target.Model.Status.Hp < minHp)
            {
                minHp = target.Model.Status.Hp;
                minHpTarget = target;
            }
        }
        
        minHpTarget?.TakeHeal(healAmount);
    }
    
    private int CalculateHealAmount(UnitController caster)
    {
        int healAmount = Mathf.RoundToInt(caster.Model.Stat.Atk * HealAmount);
        return healAmount;
    }
}
