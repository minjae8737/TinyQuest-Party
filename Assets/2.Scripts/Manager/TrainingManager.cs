using System;
using System.Collections.Generic;
using UnityEngine;

public enum TrainingType
{
    Attack,
    Defence,
    Health
}

public class TrainingManager : MonoBehaviour
{
    public static TrainingManager Instance { get; private set; }

    [SerializeField] private List<TrainingData> datas;

    private List<Stat> Stats;

    private int trainingLevel;
    private int attackLevel;
    private int defenceLevel;
    private int healthLevel;

    public int MaxTrainingLevel => datas.Count;
    
    public int TrainingLevel
    {
        get => trainingLevel;
        private set
        {
            int maxLevel = MaxTrainingLevel - 1;
            int clamp = Math.Clamp(value, 0, maxLevel);
            
            if (trainingLevel == clamp) return;
            
            trainingLevel = clamp;
            OnTrainingLevelChanged?.Invoke();
        }
    }

    public int AttackLevel
    {
        get => attackLevel;
        private set
        {
            int maxLevel = datas[trainingLevel].MaxLevel;
            int clamp = Math.Clamp(value, 0, maxLevel);
            
            if (attackLevel == clamp) return;
            
            attackLevel = clamp;
            OnAttackLevelChanged?.Invoke(attackLevel);
        }
    }

    public int DefenceLevel
    {
        get => defenceLevel;
        private set
        {
            int maxLevel = datas[trainingLevel].MaxLevel;
            int clamp = Math.Clamp(value, 0, maxLevel);
            
            if (defenceLevel == clamp) return;
            
            defenceLevel = clamp;
            OnDefenceLevelChanged?.Invoke(defenceLevel);
        }
    }

    public int HealthLevel
    {
        get => healthLevel;
        private set
        {
            int maxLevel = datas[trainingLevel].MaxLevel;
            int clamp = Math.Clamp(value, 0, maxLevel);
            
            if (healthLevel == clamp) return;
            
            healthLevel = clamp;
            OnHealthLevelChanged?.Invoke(healthLevel);
        }
    }

    public Stat TotalStat { get; private set; }
    public event Action OnTrainingLevelChanged;
    public event Action OnChangedTrainingStat;
    public event Action OnStatLevelChanged;
    public event Action<int> OnAttackLevelChanged;
    public event Action<int> OnDefenceLevelChanged;
    public event Action<int> OnHealthLevelChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Init(TrainingSaveData saveData = null)
    {
        Stats = new();

        foreach (TrainingData data in datas)
        {
            Stats.Add(new Stat());
        }
        
        ApplySaveData(saveData);
        
        // 업데이트 후 데이터가 늘어났을시 자동으로 레벨업
        TryLevelUpTrainingLevel();
        
        OnAttackLevelChanged += _ => TryLevelUpTrainingLevel();
        OnDefenceLevelChanged += _ => TryLevelUpTrainingLevel();
        OnHealthLevelChanged += _ => TryLevelUpTrainingLevel();

        OnChangedTrainingStat += UnitManager.Instance.ApplyTrainingStat;
    }

    #region LevelUp

    private void LevelUpTrainingLevel()
    {
        if (TrainingLevel >= MaxTrainingLevel - 1) return;
        
        TrainingLevel++;
        
        AttackLevel = 0; 
        DefenceLevel = 0; 
        HealthLevel = 0;
    }

    private bool IsAllLevelMax()
    {
        if (AttackLevel != GetMaxLevel(TrainingLevel)) return false;
        if (DefenceLevel != GetMaxLevel(TrainingLevel)) return false;
        if (HealthLevel != GetMaxLevel(TrainingLevel)) return false;
        
        return true;
    }

    private void TryLevelUpTrainingLevel()
    {
        if (IsAllLevelMax()) LevelUpTrainingLevel();
    }

    public void LevelUpStat(TrainingType type, int level)
    {
        TrainingData data = datas[trainingLevel];
        int startLevel = GetStatLevel(type);
        
        // 최대 레벨이면 리턴
        if (startLevel == data.MaxLevel) return;
        
        int targetLevel = Math.Clamp(startLevel + level, 0, data.MaxLevel);
        long goldCost = GetUpgradeCost(type, trainingLevel, level);

        // Gold 부족시 리턴
        if (!CurrencyManager.Instance.SpendGold(goldCost)) return;
        
        int statValue = GetIncrease(type,trainingLevel,targetLevel - startLevel);
        
        SetStatLevel(type, targetLevel);
        AddStat(type, statValue);
        AudioManager.Instance.PlaySfx(Sfx.UIUpgrade);
        OnStatLevelChanged?.Invoke();
    }

    private void SetStatLevel(TrainingType type, int targetLevel)
    {
        switch (type)
        {
            case TrainingType.Attack:
                AttackLevel = targetLevel;
                break;
            case TrainingType.Defence:
                DefenceLevel = targetLevel;
                break;
            case TrainingType.Health:
                HealthLevel = targetLevel;
                break;
        }
    }

    private int GetStatLevel(TrainingType type)
    {
        int statLevel = 0;
        
        switch (type)
        {
            case TrainingType.Attack:
                statLevel = AttackLevel;
                break;
            case TrainingType.Defence:
                statLevel = DefenceLevel;
                break;
            case TrainingType.Health:
                statLevel = HealthLevel;
                break;
        }

        return statLevel;
    }
    
    #endregion

    #region Stat

    private void AddStat(TrainingType type, int statValue)
    {
        Stat stat = Stats[TrainingLevel];
        
        switch (type)
        {
            case TrainingType.Attack:
                stat.Atk += statValue;
                break;
            case TrainingType.Defence:
                stat.Def += statValue;
                break;
            case TrainingType.Health:
                stat.MaxHp += statValue;
                break;
        }

        RefreshTotalStat();
    }

    public void RefreshTotalStat()
    {
        TotalStat = new();
        
        foreach (Stat stat in Stats)
        {
            TotalStat += stat;
        }
        
        OnChangedTrainingStat?.Invoke();
    }

    #endregion

    private long CalculateUpgradeCost(int baseCost, int startLevel, int endLevel, int costPerLevel)
    {
        if (endLevel <= startLevel) return 0;
        
        long startCost = GetLevelCost(baseCost, startLevel + 1, costPerLevel);
        long endCost = GetLevelCost(baseCost, endLevel, costPerLevel);

        int levelCount = endLevel - startLevel;

        return levelCount * (startCost + endCost) / 2;
    }

    private long GetLevelCost(int baseCost, int level, int costPerLevel)
    {
        if (level == 0) return 0;
        return baseCost + (level - 1) * costPerLevel;
    }
    
    public long GetUpgradeCost(TrainingType type, int trainingLv, int level)
    {
        TrainingData data = datas[trainingLv];
        int startLevel = 0;

        switch (type)
        {
            case TrainingType.Attack:
                startLevel = AttackLevel;
                break;
            case TrainingType.Defence:
                startLevel = DefenceLevel;
                break;
            case TrainingType.Health:
                startLevel = HealthLevel;
                break;
        }

        if (startLevel >= data.MaxLevel) return 0;

        int targetLevel = Math.Clamp(startLevel + level, 0, data.MaxLevel);


        return CalculateUpgradeCost(
            data.baseGoldCost,
            startLevel,
            targetLevel,
            data.goldCostPerLevel
        );
    }

    public int GetIncrease(TrainingType type, int trainingLv, int level)
    {
        TrainingData data = datas[trainingLv];
        int increase = 0;

        switch (type)
        {
            case TrainingType.Attack:
                increase = data.attackPerLevel * level;
                break;
            case TrainingType.Defence:
                increase = data.defencePerLevel * level;
                break;
            case TrainingType.Health:
                increase = data.healthPerLevel * level;
                break;
        }

        return increase;
    }
    
    public int GetMaxLevel(int trainingLv)
    {
        if (trainingLv >= MaxTrainingLevel) return -1;
    
        return datas[trainingLv].MaxLevel;
    }


    #region SaveData

    public TrainingSaveData GetSaveData()
    {
        return new TrainingSaveData(trainingLevel, attackLevel, defenceLevel, healthLevel);
    }

    private void ApplySaveData(TrainingSaveData saveData)
    {
        if (saveData != null)
        {
            TrainingLevel = saveData.TrainingLevel;
            AttackLevel = saveData.AttackLevel;
            DefenceLevel = saveData.DefenceLevel;
            HealthLevel = saveData.HealthLevel;
        }
    }
    
    #endregion

    #region Quest

    public int GetStatLevel(TrainingType type, int trainingLevel)
    {
        int statLevel = 0; // 다음 트레이닝 레벨

        if (this.trainingLevel == trainingLevel) // 현재 트레이닝 레벨 일때
        {
            statLevel = GetStatLevel(type);
        }
        else if(this.trainingLevel > trainingLevel) // 이전 트레이닝 레벨 일때
        {
            statLevel = GetMaxLevel(trainingLevel);
        }
        
        return statLevel;
    }

    #endregion
}
