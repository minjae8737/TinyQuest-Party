using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    [SerializeField] private List<QuestData> mainQuestDatas;
    private int curMainQuestIdx;
    private QuestData CurMainQuestData => mainQuestDatas[curMainQuestIdx];

    private QuestProgress progress;
    public event Action OnMainQuestUpdated;
    public event Action OnMainQuestClear;
    public event Action OnMainQuestRewardProvided;
    
    public void Init(QuestSaveData saveData = null)
    {
        progress = new();
        progress.Init();
        
        // 세이브데이터 적용
        ApplySaveData(saveData);
        
        StartMainQuest();

        UIManager.Instance.OnInitCompleted += CheckProgress;
    }

    #region MainQuest Cycle
    
    private void StartMainQuest()
    {
        UnitManager.Instance.OnEnemyDied -= HandleEvent;
        CurrencyManager.Instance.OnAddGold -= HandleEvent;
        TrainingManager.Instance.OnStatLevelChanged -= HandleEvent;

        if (curMainQuestIdx >= mainQuestDatas.Count)
        {
            Debug.Log("Quest End");
            UIManager.Instance.OffMainQuestPanel();
            return;
        }

        UIManager.Instance.OpenMainQuestPanel();

        switch (CurMainQuestData.Condition)
        {
            case KillUnitCondition:
                UnitManager.Instance.OnEnemyDied += HandleEvent;
                break;
            case GetCurrencyCondition:
                CurrencyManager.Instance.OnAddGold += HandleEvent;
                break;
            case TrainingLevelCondition:
                TrainingManager.Instance.OnStatLevelChanged += HandleEvent;
                break;
        }
        
        OnMainQuestUpdated?.Invoke();
        CheckProgress();
    }

    private void HandleEvent(string key, long count = 1L)
    {
        progress.Add(key, count);
        OnMainQuestUpdated?.Invoke();
        CheckProgress();
    }

    private void CheckProgress()
    {
        if (!CurMainQuestData.Condition.isSatisfied(progress)) return;
        ClearQuest();
    }

    private void ClearQuest()
    {
        OnMainQuestClear?.Invoke();
    }

    public void ProvideReward()
    {
        CurMainQuestData.Reward.Provide();
        
        OnMainQuestRewardProvided?.Invoke();
        
        curMainQuestIdx++;
        progress.Init();
        StartMainQuest();
    }
    
    #endregion

    public string GetMainQuestDesc()
    {
        return CurMainQuestData.Condition.GetDesc(progress);
    }

    public QuestReward GetRewardData()
    {
        return CurMainQuestData.Reward;
    }
    
    #region SaveData

    public QuestSaveData GetQuestSaveData()
    {
        return new QuestSaveData(
            curMainQuestIdx: curMainQuestIdx,
            savedCounter: progress.GetCurProgressData()
            );
    }

    private void ApplySaveData(QuestSaveData saveData)
    {
        if (saveData != null)
        {
            curMainQuestIdx = saveData.mainQuestIdx;

            foreach (KeyValuePair<string, long> counterPair in saveData.counter)
            {
                progress.Add(counterPair.Key,counterPair.Value);
            }
        }
    }

    #endregion
}
