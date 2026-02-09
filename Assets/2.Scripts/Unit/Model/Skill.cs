using System;
using UnityEngine;

public enum SkillType
{
    Damage, Buffs
}

public enum SkillTargetType
{
    Single,
    Circle,
    Cone,
    Line
}

public enum ProjectileStartType
{
    Target, Caster
}

public enum SkillDeliveryType
{
    Instant,    // 즉발
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
    public SkillType Type;
    public SkillTargetType TargetType;
    public SkillTargetData TargetData;
    public ProjectileStartType StartType;
    public SkillDeliveryType DeliveryType;
    
    public AnimationClip effectClip;

    public float speed;

    public float Damage;
    public float CastTime; // 선딜
    public float RecoveryTime; // 후딜
    public float Cooldown; // 재사용 대기

    public float lastUseTime;

    public bool CanUse(float curTime)
    {
        if (Cooldown > curTime - lastUseTime) return false; // 쿨타임 체크

        return true;
    }

    public void Use(float time)
    {
        lastUseTime = time;
    }
}