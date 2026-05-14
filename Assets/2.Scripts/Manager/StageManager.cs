using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState
{
    None,
    Loading,
    Intro,
    SpawnPlayer,
    WaveStart,
    Battle,
    WaveClear,
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
    private BattleState curState;
    
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

    private void SetState(BattleState state)
    {
        curState = state;
        Debug.Log($"State Changed : {state}");
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
            SetState(BattleState.Loading);
            MapManager.Instance.LoadIsland(CurStageData);

            yield return IslandLoopRoutine();
            
        }
    }
    
    private IEnumerator IslandLoopRoutine()
    {
        while (true)
        {
            yield return IslandRoutine();
        
            if (curState == BattleState.Fail)
            {
                yield return FailIslandRoutine();
                continue;
            }

            if (curState == BattleState.IslandClear)
            {
                yield return ClearIslandRoutine();
            }

            if (curState == BattleState.StageClear)
            {
                yield break;
            }
        }
    }
    
    private IEnumerator IslandRoutine()
    {
        yield return IntroRoutine();       // 시작전 연출
        yield return SpawnPlayerRoutine(); // 플레이어 유닛 스폰
        yield return WaveLoopRoutine();    // 웨이브 시작
    }

    private IEnumerator IntroRoutine()
    {
        SetState(BattleState.Intro);
        CameraManager.Instance.SetTarget(MapManager.Instance.GetCurIsland(curIslandIdx)); // 카메라 타겟 변경
        
        yield return null;
    }
    
    private IEnumerator SpawnPlayerRoutine()
    {
        SetState(BattleState.SpawnPlayer);
        Vector2 playerSpawnPos = MapManager.Instance.GetPlayerSpawnPos(curIslandIdx);
        UnitManager.Instance.SpawnParty(playerSpawnPos); // 플레이어 유닛 스폰

        yield return null;
    }

    private IEnumerator WaveLoopRoutine()
    {
        List<EnemyWave> enemyWaves = CurIslandData.Waves;
        
        for (int i = 0; i < enemyWaves.Count; i++)
        {
            yield return WaveRoutine(enemyWaves[i]);  // 한 웨이브씩 출현

            if (curState == BattleState.Fail)
            {
                yield break;
            }
        }
        
        SetState(BattleState.IslandClear);
    }

    private IEnumerator WaveRoutine(EnemyWave wave)
    {
        SetState(BattleState.WaveStart);

        Vector2 enemySpawnPos = MapManager.Instance.GetEnemySpawnPos(curIslandIdx);
        
        for (int j = 0; j < wave.SpawnCount; j++)
        {
            EnemySpawner.Instance.Spawn(wave.UnitName, enemySpawnPos);
        }

        SetState(BattleState.Battle);
        BattleManager.Instance.BattleStart();
        
        yield return new WaitUntil(() =>
            BattleManager.Instance.IsBattleEnd()
        );
        
        bool isWaveClear = BattleManager.Instance.IsWaveClear();
        BattleManager.Instance.BattleEnd();

        if (!isWaveClear)
        {
            SetState(BattleState.Fail);
            yield break;
        }
        
        SetState(BattleState.WaveClear);
        yield return ClearWaveRoutine();
    }
    
    private IEnumerator ClearWaveRoutine()
    {
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator FailIslandRoutine()
    {
        UnitManager.Instance.DespawnPlayerParty();
        yield return new WaitForSeconds(2f);
    }
    
    private IEnumerator ClearIslandRoutine()
    {
        curIslandIdx++;
        UnitManager.Instance.DespawnPlayerParty();
        
        if (IsLastIsland())
        {
            SetState(BattleState.StageClear);
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
