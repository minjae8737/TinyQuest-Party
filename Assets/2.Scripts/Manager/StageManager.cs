using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    [SerializeField] private List<Stage> stages;
    [SerializeField] private List<StageData> stageDatas;
    
    #region Runtime

    private int curStageLevel;
    private int curIslandIdx;
    
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

    #region Stage Cycle
    
    public void StartStage()
    {
        // 맵 로드
        MapManager.Instance.LoadIsland(CurStageData);
        
        StartIsland();
    }
    
    private void StartIsland()
    {
        // 다음 Island가 없다면 다음 Stage로
        if (IsLastIsland())
        {
            curIslandIdx = 0;
            NextStage();
            return;
        }
        
        Vector2 playerSpawnPos = MapManager.Instance.GetPlayerSpawnPos(curIslandIdx);
        
        // 카메라 타겟 변경
        CameraManager.Instance.SetTarget(MapManager.Instance.GetCurIsland(curIslandIdx));
        
        // 유닛 생성
        UnitManager.Instance.SpawnParty(playerSpawnPos);
        SpawnEnemy(CurStageData);

        // 전투 시작
        BattleManager.Instance.BattleStart();
    }

    private void SpawnEnemy(StageData stageData)
    {
        // TODO 나중에 Wave 별로 시간차로 스폰될수 있게 변경
        // 지금은 한꺼번에 스폰
        List<EnemyWave> enemyWaves = CurIslandData.Waves;
        Vector2 enemySpawnPos = MapManager.Instance.GetEnemySpawnPos(curIslandIdx);

        for (int i = 0; i < enemyWaves.Count; i++)
        {
            for (int j = 0; j < enemyWaves[i].SpawnCount; j++)
            {
                EnemySpawner.Instance.Spawn(enemyWaves[i].UnitName, enemySpawnPos);
            }
        }
    }

    public void OnIslandClear()
    {
        curIslandIdx++;
        StartCoroutine(NextIslnad());
    }

    public void OnIslandFail()
    {
        StartCoroutine(NextIslnad());
    }
    
    private IEnumerator NextIslnad()
    {
        yield return new WaitForSeconds(2f);
        StartIsland();
    }

    private void NextStage()
    {
        curStageLevel++;
        
        if (IsLastStage())
        {
            curStageLevel = stageDatas.Count - 1;
        }

        StartStage();
    }

    private bool IsLastIsland()
    {
        return curIslandIdx >= CurStageData.IslandDatas.Count;
    }

    private bool IsLastStage()
    {
        return curStageLevel >= stageDatas.Count - 1;
    }
    
    #endregion

    public void RequestStageReward(Vector3 unitPos)
    {
        RewardData rewardData = CurStageData.RewardData;

        GameManager.Instance.DropReward(rewardData, unitPos);
    }

    #region SaveData

    public StageSaveData GetPartySaveData()
    {
        return new StageSaveData(curStageLevel, curIslandIdx);
    }

    #endregion
}
