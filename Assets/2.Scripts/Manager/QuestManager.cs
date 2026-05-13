using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    [SerializeField] private List<QuestData> mainQuestDatas;
    private int curMainQuestIdx;
    private QuestData CurMainQuestData => mainQuestDatas[curMainQuestIdx];

    private QuestProgress progress;
    public event Action OnMainQuestUpdated;
    public event Action OnMainQuestClear;
    public event Action OnMainQuestRewardProvided;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    public void Init()
    {
        progress = new();
        progress.Init();
        
        curMainQuestIdx = 0;
        StartMainQuest();
    }

    #region MainQuest Cycle
    
    private void StartMainQuest()
    {
        UnitManager.Instance.OnEnemyDied -= HandleEvent;
        CurrencyManager.Instance.OnAddGold -= HandleEvent;
        TrainingManager.Instance.OnStatLevelChanged -= CheckProgress;

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
                TrainingManager.Instance.OnStatLevelChanged += CheckProgress;
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

    

    #endregion
}
