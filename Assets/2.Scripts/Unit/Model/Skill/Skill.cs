using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SkillType
{
    Damage,
    Heal,
    Buff,
    Debuff
}

public enum  SkillTargetType
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

public enum EffectSpawnType
{
    Primary,
    EachTarget,
    Caster,
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
    #region Data
    
    public SkillData Data;
    public SkillTargetScanner scanner;
    
    #endregion 
    
    // Runtime
    private float lastUseTime;
    
    // Cache
    private SkillScanResult scanResult;

    public SkillScanResult ScanResult => scanResult;


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
        scanResult = scanner.Scan(caster, Data.TargetData);

        return scanResult.Targets.Count > 0;
    }

    public void Use(UnitController caster)
    {
        // 이펙트, 발사체 세팅
        if (Data.EffectClip != null)
        {
            PlaySkillEffect(caster);
        }
        else
        {
            Data.Use(caster, scanResult.Targets);
        }

        RefreshLastUseTime();
    }

    private void PlaySkillEffect(UnitController caster)
    {
        List<SkillEffect> skillEffects = new();
        
        // Select EffectPos
        switch (Data.EffectSpawnType)
        {
            case EffectSpawnType.Primary:
                scanResult.SpawnPos.Add(scanResult.PrimaryTarget.transform.position);
                break;
            
            //TODO LINQ 개선
            case EffectSpawnType.EachTarget:
                scanResult.SpawnPos = scanResult.Targets.Select(u => (Vector2)u.transform.position).ToList();
                break;
            
            case EffectSpawnType.Caster:
                scanResult.SpawnPos.Add(caster.transform.position);
                break;
        }
        
        // 이펙트 타겟위치 세팅
        //TODO LINQ 개선
        scanResult.TargetPos = scanResult.Targets.Select(u => (Vector2)u.transform.position).ToList();
        
        // SkillEffect 생성
        foreach (Vector2 pos in scanResult.SpawnPos)
        {
            SkillEffect newSkillEffect = UnitManager.Instance.SpawnSkillEffect(pos);
            skillEffects.Add(newSkillEffect);
        }
        
        float arrivedTime = 0f;

        // 발사체일경우 세팅
        if (Data.DeliveryType == SkillDeliveryType.Projectile)
        {
            for (int i = 0; i < skillEffects.Count; i++)
            {
                if (skillEffects[i].TryGetComponent<ProjectileMover>(out var mover))
                {
                    ProjectileDamageSkillData projectileDamageSkillData = Data as ProjectileDamageSkillData;

                    mover.Init(scanResult.SpawnPos[i], scanResult.TargetPos[i],
                        projectileDamageSkillData.Speed, () =>
                        {
                            // 발사체 데미지 적용
                            Data.Use(caster, scanResult.Targets);
                        }
                    );

                    arrivedTime = mover.GetArrivedTime();
                }
            }
        }
        else
        {
            Data.Use(caster, scanResult.Targets);
        }
        
        // SkillEffect 재생
        foreach (SkillEffect skillEffect in skillEffects)
        {
            skillEffect.Play(Data.EffectClip, arrivedTime);
        }
    }
}