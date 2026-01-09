using System;
using UnityEngine;

public enum SkillTargetType
{
    Single, Circle, Cone, Line
}

[Serializable]
public class Skill
{
    public SkillTargetType SkillTargetType;
    public int Damage;
    public float Range;
    
    public float CastTime;       // 선딜
    public float RecoveryTime;   // 후딜
    public float Cooldown;       // 재사용 대기
    
    public float lastUseTime;
    
    public bool CanUse(Vector2 curPos,Vector2 targetPos, float curTime)
    {
        if (Range < Vector2.Distance(curPos, targetPos)) return false;
        if (Cooldown > curTime - lastUseTime) return false;
        
        return true;
    }
    
    public void Use(float time)
    {
        lastUseTime = time;
    }
    
}
