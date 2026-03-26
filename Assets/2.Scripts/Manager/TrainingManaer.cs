using System;
using System.Collections.Generic;
using UnityEngine;

public enum TrainingType
{
    Attack,
    Defence,
    Health
}

public class TrainingManaer : MonoBehaviour
{
    public static TrainingManaer Instance { get; private set; }

    [SerializeField] private List<TrainingData> datas;

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
            
            // if (trainingLevel == clamp) return;
            
            trainingLevel = clamp;
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

    private Stat totalStat;
    
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

    #region LevelUp

    private void LevelUpTrainingLevel()
    {
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

    public void LevelUpAttack(int level)
    {
        TrainingData data = datas[trainingLevel];

        // 최대 레벨이면 리턴
        if (AttackLevel == data.MaxLevel) return;
        
        int targetLevel = Math.Clamp(AttackLevel + level, 0, data.MaxLevel);
        long goldCost = GetAttackUpgradeCost(trainingLevel, level);

        // Gold 부족시 리턴
        if (!CurrencyManager.Instance.SpendGold(goldCost)) return;

        AttackLevel = targetLevel;
    }

    public void LevelUpDefence(int level)
    {
        TrainingData data = datas[trainingLevel];

        // 최대 레벨이면 리턴
        if (DefenceLevel == data.MaxLevel) return;
        
        int targetLevel = Math.Clamp(DefenceLevel + level, 0, data.MaxLevel);
        long goldCost = GetDefenceUpgradeCost(trainingLevel, level);

        // Gold 부족시 리턴
        if (!CurrencyManager.Instance.SpendGold(goldCost)) return;

        DefenceLevel = targetLevel;
    }

    public void LevelUpHealth(int level)
    {
        TrainingData data = datas[trainingLevel];

        // 최대 레벨이면 리턴
        if (HealthLevel == data.MaxLevel) return;
        
        int targetLevel = Math.Clamp(HealthLevel + level, 0, data.MaxLevel);
        long goldCost = GetHealthUpgradeCost(trainingLevel, level);

        // Gold 부족시 리턴
        if (!CurrencyManager.Instance.SpendGold(goldCost)) return;

        HealthLevel = targetLevel;
    }
    
    #endregion

    #region Stat

    

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
    
    public long GetAttackUpgradeCost(int trainingLv, int level)
    {
        TrainingData data = datas[trainingLv];

        if (AttackLevel >= data.MaxLevel) return 0;

        int targetLevel = Math.Clamp(AttackLevel + level, 0, data.MaxLevel);

        return CalculateUpgradeCost(
            data.baseGoldCost,
            AttackLevel,
            targetLevel,
            data.goldCostPerLevel
        );
    }
    
    public long GetDefenceUpgradeCost(int trainingLv, int level)
    {
        TrainingData data = datas[trainingLv];

        if (DefenceLevel >= data.MaxLevel) return 0;

        int targetLevel = Math.Clamp(DefenceLevel + level, 0, data.MaxLevel);

        return CalculateUpgradeCost(
            data.baseGoldCost,
            DefenceLevel,
            targetLevel,
            data.goldCostPerLevel
        );
    }

    public long GetHealthUpgradeCost(int trainingLv, int level)
    {
        TrainingData data = datas[trainingLv];

        if (HealthLevel >= data.MaxLevel) return 0;

        int targetLevel = Math.Clamp(HealthLevel + level, 0, data.MaxLevel);

        return CalculateUpgradeCost(
            data.baseGoldCost,
            HealthLevel,
            targetLevel,
            data.goldCostPerLevel
        );
    }

    public int GetAttackIncrease(int trainingLevel, int level)
    {
        TrainingData data = datas[trainingLevel];
        return data.attackPerLevel * level;
    }
    
    public int GetDefenceIncrease(int trainingLevel, int level)
    {
        TrainingData data = datas[trainingLevel];
        return data.defencePerLevel * level;
    }
    
    public int GetHealthIncrease(int trainingLevel, int level)
    {
        TrainingData data = datas[trainingLevel];
        return data.healthPerLevel * level;
    }

    public int GetMaxLevel(int trainingLevel)
    {
        if (trainingLevel >= MaxTrainingLevel) return -1;

        return datas[trainingLevel].MaxLevel;
    }
}
