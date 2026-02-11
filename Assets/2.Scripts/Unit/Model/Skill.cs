using System;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Damage,
    Heal,
    Buff,
    Debuff
}

public enum SkillTargetType
{
    Single,
    Circle,
    Cone,
    Line
}

public enum SkillDeliveryType
{
    Instant, // 즉발
    Projectile, // 발차세
}

public enum SkillSlot
{
    Normal,
    Skill1,
    Skill2,
    Skill3
}

[Serializable]
public class Skill
{
    public SkillData Data;
    public SkillTargetScanner scanner;
    private float lastUseTime;
    private List<UnitController> targets = new();
    public Vector2 TargetPos => scanner.TargetPos;

    public bool CanUse(float curTime)
    {
        if (Data.Cooldown > curTime - lastUseTime) return false; // 쿨타임 체크

        return true;
    }

    public void RefreshLastUseTime()
    {
        lastUseTime = Time.time;
    }

    public bool CanCast(UnitController caster)
    {
        targets = scanner.Scan(caster, Data);

        return targets.Count > 0;
    }

    public void Use(UnitController caster)
    {
        // 이펙트, 발사체 세팅
        if (Data.effectClip != null)
        {
            PlaySkillEffect(caster, targets);
        }
        else
        {
            Data.Use(caster, targets);
        }

        RefreshLastUseTime();
    }

    private void PlaySkillEffect(UnitController caster, List<UnitController> targets)
    {
        Vector2 effectPos = scanner.TargetPos;

        SkillEffect skillEffect = UnitManager.Instance.SpawnSkillEffect(effectPos);
        float arrivedTime = 0f;

        if (Data.DeliveryType == SkillDeliveryType.Projectile && skillEffect.TryGetComponent<ProjectileMover>(out var mover))
        {
            ProjectileDamageSkillData projectileDamageSkillData = Data as ProjectileDamageSkillData;

            mover.Init(scanner.TargetPos, projectileDamageSkillData.Speed, () =>
                {
                    // 발사체 데미지 적용
                    Data.Use(caster, targets);
                }
            );
            arrivedTime = mover.GetArrivedTime();
        }
        else
        {
            Data.Use(caster, targets);
        }

        skillEffect.Play(Data.effectClip, arrivedTime);
    }
}