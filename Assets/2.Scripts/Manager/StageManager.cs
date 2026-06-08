using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StageState
{
    None,
    Loading,
    Intro,
    IslandClear,
    StageClear,
    Fail,
}

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    [SerializeField] private List<Stage> stages;
    [SerializeField] private List<StageData> stageDatas;

    private const float ScalingRate = 1.05f;
    private const float GoldRate = 1.05f;
    private const float ExpRate = 1.06f;
    private const int BaseGold = 100;
    private const int BaseExp = 100;
    
    #region Runtime

    private float stageProgress;
    
    private int curStageLevel;
    private int curIslandIdx;
    private StageState curState;
    
    private StageData CurStageData => GetCurStageData();
    private IslandData CurIslandData => CurStageData.IslandDatas[curIslandIdx];
    
    public int CurStageLevel => curStageLevel;
    public int CurIslandIdx => curIslandIdx;
    public int CurIslandCount => CurStageData.IslandDatas.Count;

    #endregion

    public event Action OnStageChanged;
    public event Action OnChangedIsland;

    public event Action OnChangedProgress
    {
        add => BattleManager.Instance.OnWaveChanged += value;
        remove => BattleManager.Instance.OnWaveChanged -= value;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Init(StageSaveData saveData = null)
    {
        curStageLevel = saveData.CurStageLevel;
        curIslandIdx = saveData.CurIslandIdx;
    }

    private void SetState(StageState state)
    {
        curState = state;
        // Debug.Log($"State Changed : {state}");
    }

    private StageData GetCurStageData()
    {
        foreach (StageData data in stageDatas)
        {
            if (data.ContainsLevel(curStageLevel))
            {
                return data;
            }
        }

        return stageDatas[stageDatas.Count - 1];
    }

    private int GetMaxStageLevel()
    {
        int maxLevel = 0;
        
        foreach (StageData data in stageDatas)
        {
            if (data.endStage > maxLevel)
            {
                maxLevel = data.endStage;
            }
        }

        return maxLevel;
    }

    #region Stage Cycle
    
    public void StartStage()
    {
        StartCoroutine(StageRoutine());
    }

    private IEnumerator StageRoutine()
    {
        while (true)
        {
            SetState(StageState.Loading);
            MapManager.Instance.LoadIsland(CurStageData);

            yield return IslandLoopRoutine();
        }
    }
    
    private IEnumerator IslandLoopRoutine()
    {
        while (true)
        {
            yield return IslandRoutine();
        
            if (curState == StageState.Fail)
            {
                yield return FailIslandRoutine();
                continue;
            }

            if (curState == StageState.IslandClear)
            {
                yield return ClearIslandRoutine();
            }

            if (curState == StageState.StageClear)
            {
                yield break;
            }
        }
    }
    
    private IEnumerator IslandRoutine()
    {
        yield return IntroRoutine();       // 시작전 연출
        yield return WaveLoopRoutine();    // 웨이브 시작
    }

    private IEnumerator IntroRoutine()
    {
        SetState(StageState.Intro);
        CameraManager.Instance.SetTarget(MapManager.Instance.GetCurIsland(curIslandIdx)); // 카메라 타겟 변경

        OnStageChanged?.Invoke();
        OnChangedIsland?.Invoke();
        
        yield return null;
    }

    private IEnumerator WaveLoopRoutine()
    {
        StageData stageData = CurStageData;

        bool isVictory = false;

        List<WavePattern> patterns = new();
        
        for (int i = 0; i < stageData.WaveCount; i++)
        {
            WaveType type = stageData.DecideWaveType(curIslandIdx, i);
            WavePattern pattern = stageData.PickUpPattern(type);
            
            patterns.Add(pattern);
        }
        
        yield return BattleManager.Instance.WaveLoopRoutine(patterns, curIslandIdx, result => isVictory = result);
        

        if (isVictory) yield return ClearIslandRoutine();
        else yield return FailIslandRoutine();
    }

    private IEnumerator FailIslandRoutine()
    {
        yield return new WaitForSeconds(2f);
    }
    
    private IEnumerator ClearIslandRoutine()
    {
        curIslandIdx++;
        
        if (IsLastIsland())
        {
            SetState(StageState.StageClear);
            curIslandIdx = 0;
            curStageLevel++;
            curStageLevel = Math.Clamp(curStageLevel, 0, GetMaxStageLevel());
        }
        
        yield return new WaitForSeconds(2f);
    }

    private bool IsLastIsland()
    {
        return curIslandIdx >= CurStageData.islandCount;
    }
    
    #endregion

    public float GetStageProgress()
    {
        float stagestep = 1f / CurIslandCount;
        float progress = stagestep * CurIslandIdx + (stagestep * BattleManager.Instance.Progress); // 스테이지 진행도 + 웨이브 진행도
        
        return progress;
    }

    public void RequestStageReward(Vector3 unitPos)
    {
        RewardData rewardData = CurStageData.RewardData;
        
        rewardData.Gold = (long)Math.Round(BaseGold * GetGoldScaling());
        rewardData.Exp = (long)Math.Round(BaseExp * GetExpScaling());

        GameManager.Instance.DropReward(rewardData, unitPos);
    }

    public float GetStageScaling()
    {
        return Mathf.Pow(ScalingRate, curStageLevel); 
    }

    public float GetGoldScaling()
    {
        return Mathf.Pow(GoldRate, curStageLevel); 
    }
    
    public float GetExpScaling()
    {
        return Mathf.Pow(ExpRate, curStageLevel); 
    }

    #region SaveData

    public StageSaveData GetPartySaveData()
    {
        return new StageSaveData(curStageLevel, curIslandIdx);
    }

    #endregion
}
