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
    public float Damage;
    public float Range;
    [Range(0, 360)] public float Angle;
    
    
    public float CastTime;       // 선딜
    public float RecoveryTime;   // 후딜
    public float Cooldown;       // 재사용 대기
    
    public float lastUseTime;
    
    public bool CanUse(float curTime)
    {
        // Vector2 diff = targetPos - curPos; 
        // if (Range * Range < diff.sqrMagnitude) return false; // 거리 체크 
        if (Cooldown > curTime - lastUseTime) return false;  // 쿨타임 체크
        
        return true;
    }
    
    public void Use(float time)
    {
        lastUseTime = time;
    }
    
}
