using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    [SerializeField] private List<QuestData> mainQuestDatas;
    private int curMainQuestIdx;
    private QuestData CurMainQuestData => mainQuestDatas[curMainQuestIdx];

    private QuestProgress progress;

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
        if (curMainQuestIdx >= mainQuestDatas.Count)
        {
            Debug.Log("Quest End");
            return;
        }

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
    }

    private void HandleEvent(string key, long count = 1L)
    {
        progress.Add(key, count);
        CheckProgress();
    }

    private void CheckProgress()
    {
        if (!CurMainQuestData.Condition.isSatisfied(progress)) return;
        ClearQuest();
    }

    private void ClearQuest()
    {
        curMainQuestIdx++;
        progress.Init();

        StartMainQuest();
    }
    
    #endregion

    #region SaveData

    

    #endregion
}
