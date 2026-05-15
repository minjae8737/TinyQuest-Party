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

    private const float ScalingRate = 1.15f;
    
    #region Runtime

    private int curStageLevel;
    private int curIslandIdx;
    private StageState curState;
    
    private StageData CurStageData => stageDatas[curStageLevel];
    private IslandData CurIslandData => CurStageData.IslandDatas[curIslandIdx];

    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Init(StageSaveData saveData = null)
    {
        curStageLevel = saveData != null ? saveData.CurStageLevel : 0;
        curIslandIdx = saveData != null ? saveData.CurIslandIdx : 0;
    }

    private void SetState(StageState state)
    {
        curState = state;
        // Debug.Log($"State Changed : {state}");
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

        yield return null;
    }

    private IEnumerator WaveLoopRoutine()
    {
        List<EnemyWave> enemyWaves = CurIslandData.Waves;
        bool isVictory = false;
        
        yield return BattleManager.Instance.WaveLoopRoutine(enemyWaves, curIslandIdx, result => isVictory = result);

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
            curStageLevel = Math.Clamp(curStageLevel, 0, stageDatas.Count - 1);
        }
        
        yield return new WaitForSeconds(2f);
    }

    private bool IsLastIsland()
    {
        return curIslandIdx >= CurStageData.IslandDatas.Count;
    }
    
    #endregion

    public void RequestStageReward(Vector3 unitPos)
    {
        RewardData rewardData = CurStageData.RewardData;

        GameManager.Instance.DropReward(rewardData, unitPos);
    }

    public float GetStageScaling()
    {
        return Mathf.Pow(ScalingRate, curStageLevel); 
    }

    #region SaveData

    public StageSaveData GetPartySaveData()
    {
        return new StageSaveData(curStageLevel, curIslandIdx);
    }

    #endregion
}
