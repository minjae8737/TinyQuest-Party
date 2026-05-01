using System;
using System.Collections.Generic;
using UnityEngine;

public enum TeamType
{
    Player,
    Enemy
}

public enum UnitClass
{
    Tank,
    Warrior,
    Archer,
    Mage,
    Healer,
}

public enum UnitGrade
{
    Normal,     // 흰,회
    Rare,       // 하늘
    Epic,       // 보라
    Legendary,  // 노랑,주황, 금
    Mythic      // 무지개
}

[Serializable]
public class Unit
{
    #region Components
    
    public TeamType TeamType => Data.TeamType;
    public UnitData Data;
    public UnitLevel Level;
    public UnitStat Stat;
    public UnitEquipment Equipment;
    public UnitStatus Status;
    
    public List<Skill> Skills;
    
    #endregion

    public UnitGrade UnitGrade;
    private int starGrade;
    public int StarGrade
    {
        get => starGrade;
        set
        {
            starGrade = value;
            Math.Clamp(starGrade, 1, 5);
        }
    }
    
    public bool IsDeath => Status.IsDeath;
    
    #region Init

    public void Init(UnitSaveData saveData)
    {
        Level.Init();

        StarGrade = TeamType == TeamType.Player ? (Data as PlayerUnitData).StartStarGrade : 1;
        // TODO UnitGrade 나중에 로직 추가
        UnitGrade = UnitGrade.Normal;
        
        if (saveData != null)
        {
            ApplySaveData(saveData);
        }

        Stat.SetBaseStat(UnitStatCalculator.GetBaseStat(Data, StarGrade, StarGrade));
        Stat.SetTrainingStat(UnitStatCalculator.GetTrainingStat(Data));
        
        Stat.RefreshStat();
        Status.Init(Stat.MaxHp, Stat.MaxHp);
    }

    public void ApplySaveData(UnitSaveData saveData)
    {
        // Load SaveData
        // UnitLevel
        Level.Level = saveData.Level;
        Level.Exp = saveData.Exp;
        
        // Equipment
        foreach (KeyValuePair<EquipPart, string> equipment in saveData.Equipments)
        {
            string itemId = equipment.Value;
            PutOnEquipment(itemId);
        }
    }

    public void ApplyTariningStat(Stat stat)
    {
        Stat.SetTrainingStat(stat);
        long roseHp = Status.MaxHp - Status.Hp;
        Debug.Log($"{Data.UnitName} Apply");
        Status.Init(Stat.MaxHp - roseHp, Stat.MaxHp);
    }
    
    #endregion

    #region Equipment
    
    public void PutOnEquipment(string itemId)
    {
        Item item = ItemManager.Instance.Get(itemId);
        if (item == null) return;

        EquipmentData equipmentData = ItemManager.Instance.GetData(item.DataId) as EquipmentData;
        if (equipmentData == null) return;

        EquipPart equipPart = equipmentData.Part;

        string preItemId = Equipment.GetEquipmentId(equipPart);
        if (!preItemId.Equals("EmptyId"))
        {
            RemoveEquipment(preItemId);
        }

        Equipment.SetEquipment(equipPart, item.DataId);
        Stat.EquipStat.Add(equipmentData.Stat);
        Stat.RefreshStat();
    }

    public void RemoveEquipment(string itemId)
    {
        Item item = ItemManager.Instance.Get(itemId);
        if (item == null) return;

        EquipmentData equipmentData = ItemManager.Instance.GetData(item.DataId) as EquipmentData;
        if (equipmentData == null) return;
        
        EquipPart equipPart = equipmentData.Part;
        
        Equipment.RemoveEquipment(equipPart);
        Stat.EquipStat.Subtrack(equipmentData.Stat);
        Stat.RefreshStat();
    }
    
    #endregion

    #region Combat

    public float TakeDamage(long damage)
    {
        if (damage - Stat.Def < 0) return 0;
        damage -= Stat.Def;
        Status.TakeDamage(damage);
        
        return damage;
    }

    public void TakeHeal(long healAmount)
    {
        Status.TakeHeal(healAmount);
    }

    #endregion

    #region Skill

    public Skill GetSkill(int skillIdx)
    {
        if (skillIdx >= Skills.Count) return null;
        return Skills[skillIdx];
    }

    #endregion

    #region Event
    
    public event Action<int> OnLevelChanged
    {
        add => Level.OnLevelChanged += value;
        remove => Level.OnLevelChanged -= value;
    }

    public event Action<long, long> OnHpChanged
    {
        add => Status.OnHpChanged += value;
        remove => Status.OnHpChanged -= value;
    }

    public event Action<int> OnAtkChanged
    {
        add => Stat.OnAtkChanged += value;
        remove => Stat.OnAtkChanged -= value;
    }

    public event Action<int> OnDefChanged
    {
        add => Stat.OnDefChanged += value;
        remove => Stat.OnDefChanged -= value;
    }

    public event Action<int> OnSpeedChanged
    {
        add => Stat.OnSpeedChanged += value;
        remove => Stat.OnSpeedChanged -= value;
    }

    #endregion
    
    #region Save

    public UnitSaveData GetSaveData()
    {
        UnitSaveData saveData = new UnitSaveData();
        
        saveData.Level = Level.Level;
        saveData.Exp = Level.Exp;

        saveData.Equipments = new Dictionary<EquipPart, string>(Equipment.Equipments);

        return saveData;
    }
    
    #endregion
}