using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState
{
    Idle,
    Spawning,
    Fighting,
}

public enum BattleResult
{
    None,
    WaveClear,
    AllWaveClear,
    Defeat
}

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    [Range(0.2f, 0.5f)] [SerializeField] private float delay;
    private float lastTime;
    private BattleState curState;
    private BattleResult result;

    private float progress;

    public float Progress
    {
        get => progress;
        private set
        {
            progress = value;
            OnWaveChanged?.Invoke();
        }
    }

    public event Action OnWaveChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    private void SetBattleState(BattleState state)
    {
        curState = state;
        // Debug.Log($"State Changed : {state}");
    }
    
    private void SetResultState(BattleResult state)
    {
        result = state;
        // Debug.Log($"State Changed : {state}");
    }

    #region BattleCycle
    
    private void  BattleStart()
    {
        UnitManager.Instance.CombatEnabled(true);
    }

    public IEnumerator WaveLoopRoutine(List<WavePattern> patterns, int curIslandIdx, Action<bool> onComplete)
    {
        SetBattleState(BattleState.Spawning);
        Vector2 playerSpawnPos = MapManager.Instance.GetPlayerSpawnPos(curIslandIdx);
        SpawnPlayer(playerSpawnPos);

        Progress = 0f;
        
        for (int i = 0; i < patterns.Count; i++)
        {
            yield return WaveRoutine(patterns[i], curIslandIdx);  // 한 웨이브씩 출현
            
            if (result == BattleResult.Defeat)
            {
                DespawnEnemy();
                onComplete?.Invoke(false);
                yield break;
            }

            Progress = (float)(i + 1) / patterns.Count;
        }
        
        SetResultState(BattleResult.AllWaveClear);
        DespawnPlayer();
        onComplete?.Invoke(true);
    }

    private IEnumerator WaveRoutine(WavePattern pattern, int curIslandIdx)
    {
        Vector2 enemySpawnPos = MapManager.Instance.GetEnemySpawnPos(curIslandIdx);
        
        SpawnEnemy(pattern, enemySpawnPos);

        SetBattleState(BattleState.Fighting);
        Instance.BattleStart();
        
        yield return new WaitUntil(() =>
            Instance.IsBattleEnd()
        );
        
        bool isWaveClear = Instance.IsWaveClear();
        Instance.BattleEnd();

        if (!isWaveClear)
        {
            SetResultState(BattleResult.Defeat);
            yield break;
        }
        
        SetResultState(BattleResult.WaveClear);
        yield return ClearWaveRoutine();
    }

    private IEnumerator ClearWaveRoutine()
    {
        UnitManager.Instance.DespawnEnemyParty();
        yield return new WaitForSeconds(1f);
    }

    private void BattlePause()
    {
        UnitManager.Instance.CombatEnabled(false);
    }

    private bool IsBattleEnd()
    {
        return !IsBattleOngoing();
    }

    private void BattleEnd()
    {
        BattlePause();
    }

    #endregion

    private static void SpawnPlayer(Vector2 playerSpawnPos)
    {
        UnitManager.Instance.SpawnParty(playerSpawnPos);
    }

    private static void DespawnPlayer()
    {
        UnitManager.Instance.DespawnPlayerParty(); 
    }

    private static void SpawnEnemy(WavePattern pattern, Vector2 enemySpawnPos)
    {
        EnemySpawner.Instance.Spawn(pattern, enemySpawnPos);
    }

    private static void DespawnEnemy()
    {
        UnitManager.Instance.DespawnPlayerParty(); 
    }

    private bool IsWaveClear()
    {
        return UnitManager.Instance.TeamAliveCount[TeamType.Player] > 0;
    }

    private bool IsBattleOngoing()
    {
        return UnitManager.Instance.TeamAliveCount[TeamType.Player] > 0 && UnitManager.Instance.TeamAliveCount[TeamType.Enemy] > 0;
    }
}
