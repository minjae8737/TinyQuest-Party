using System;
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

    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Init()
    {
        // 세이브 데이터 로드
        curStageLevel = 1;
    }
    
    public void StartStage()
    {
        if (curStageLevel >= stageDatas.Count)
        {
            curStageLevel = stageDatas.Count - 1; // 오버플로우 방지
        }
        
        curStageLevel = 0;
        curIslandIdx = 0;
        
        // 맵 로드
        MapManager.Instance.LoadIsland(stageDatas[curStageLevel]);
        
        StartIsland();
    }
    
    private void StartIsland()
    {
        // 다음 Island가 없다면 다음 Stage로
        if (IsLastIsland())
        {
            NextStage();
            return;
        }

        Vector2 playerSpawnPos = MapManager.Instance.GetPlayerSpawnPos(curIslandIdx);
        
        // 카메라 타겟 변경
        CameraManager.Instance.SetTarget(MapManager.Instance.GetCurIsland(curIslandIdx));
        
        // 유닛 생성
        UnitManager.Instance.SpawnParty(playerSpawnPos);
        SpawnEnemy(stageDatas[curStageLevel]);

        // 전투 시작
        BattleManager.Instance.BattleStart();
    }

    public void SpawnEnemy(StageData stageData)
    {
        // TODO 나중에 Wave 별로 시간차로 스폰될수 있게 변경
        // 지금은 한꺼번에 스폰
        List<EnemyWave> enemyWaves = stageData.IslandDatas[curIslandIdx].Waves;
        Vector2 enemySpawnPos = MapManager.Instance.GetEnemySpawnPos(curIslandIdx);

        for (int i = 0; i < enemyWaves.Count; i++)
        {
            for (int j = 0; j < enemyWaves[i].SpawnCount; j++)
            {
                EnemySpawner.Instance.Spawn(enemyWaves[i].UnitName, enemySpawnPos);
            }
        }
    }

    public void NextStage()
    {
        if (IsLastStage())
        {
            curStageLevel = stageDatas.Count - 1; // 오버플로우 방지
        }

        curStageLevel++;
        StartStage();
    }

    public void OnIslandClear()
    {
        curIslandIdx++;
        StartIsland();
    }

    public void OnIslandFail()
    {
        StartIsland();
    }

    private bool IsLastIsland()
    {
        return curIslandIdx >= stageDatas[curStageLevel].IslandDatas.Count;
    }

    private bool IsLastStage()
    {
        return curStageLevel >= stageDatas.Count;
    }

    public void RequestStageReward(Vector3 unitPos)
    {
        RewardData rewardData = stageDatas[curStageLevel].RewardData;

        GameManager.Instance.DropReward(rewardData, unitPos);
    }
}
