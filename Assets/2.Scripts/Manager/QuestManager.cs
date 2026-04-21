using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    [SerializeField] private List<QuestData> questDatas;
    private int curQuestIdx;

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
        
        curQuestIdx = 0;
        SetQuest();
    }

    private void SetQuest()
    {
        
    }

    private void CheckProgress()
    {
        
    }

    private void ClearQuest()
    {
        
    }
}
