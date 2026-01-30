using System;
using System.Collections.Generic;

[Serializable]
public class Unit
{
    public UnitData Data;
    public UnitLevel Level;
    public UnitStat Stat;
    public UnitEquipment Equipment;
    public UnitStatus Status;
    public bool IsDeath => Status.IsDeath;

    public List<Skill> Skills;

    public void Init(UnitSaveData saveData)
    {
        Level.Init();
        Stat.BaseStat = Data.BaseStat.Clone();
        
        if (saveData != null)
        {
            ApplySaveData(saveData);
        }

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
        foreach (KeyValuePair<EquipPart, long> equipment in saveData.Equipments)
        {
            long itemId = equipment.Value;
            PutOnEquipment(itemId);
        }
    }

    public void PutOnEquipment(long itemId)
    {
        Item item = ItemManager.Instance.Get(itemId);
        if (item == null) return;

        EquipmentData equipmentData = ItemManager.Instance.GetData(item.DataId) as EquipmentData;
        if (equipmentData == null) return;

        EquipPart equipPart = equipmentData.Part;

        long preItemId = Equipment.GetEquipmentId(equipPart);
        if (preItemId != -1)
        {
            RemoveEquipment(preItemId);
        }

        Equipment.SetEquipment(equipPart, item.Id);
        Stat.EquipStat.Add(equipmentData.Stat);
        Stat.RefreshStat();
    }

    public void RemoveEquipment(long itemId)
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

    public void TakeDamage(int damage)
    {
        if (damage - Stat.Def < 0) return;
        damage -= Stat.Def;
        Status.TakeDamage(damage);
    }

    public Skill GetSkill(int skillIdx)
    {
        if (skillIdx >= Skills.Count) return null;
        return Skills[skillIdx];
    }

    public event Action<int> OnLevelChanged
    {
        add => Level.OnLevelChanged += value;
        remove => Level.OnLevelChanged -= value;
    }

    public event Action<int, int> OnHpChanged
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

    public UnitSaveData GetSaveData()
    {
        UnitSaveData saveData = new UnitSaveData();
        
        saveData.Level = Level.Level;
        saveData.Exp = Level.Exp;

        saveData.Equipments = new Dictionary<EquipPart, long>(Equipment.Equipments);

        return saveData;
    }
}