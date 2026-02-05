using System;
using UnityEngine;

public enum SkillTargetType
{
    Single,
    Circle,
    Cone,
    Line
}

public enum SkillType
{
    BasicAttack,
    Skill1,
    Skill2,
    Skill3
}

[Serializable]
public class Skill
{
    public SkillTargetType SkillTargetType;
    public float Damage;
    [SerializeReference] public SkillTargetData TargetData;
    public AnimationClip effectClip;

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