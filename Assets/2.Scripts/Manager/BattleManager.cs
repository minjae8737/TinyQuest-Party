using System;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    [Range(0.2f, 0.5f)] [SerializeField] private float delay;
    private float lastTime;
    private bool isBattle;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        isBattle = false;
    }

    public void  BattleStart()
    {
        UnitManager.Instance.CombatEnabled(true);
        isBattle = true;
    }

    public void BattlePause()
    {
        UnitManager.Instance.CombatEnabled(false);
        isBattle = false;
    }

    public bool IsBattleEnd()
    {
        return !IsBattleOngoing();
    }

    public void BattleEnd()
    {
        BattlePause();

        UnitManager.Instance.DespawnEnemyParty();
    }

    public bool IsWaveClear()
    {
        return UnitManager.Instance.TeamAliveCount[TeamType.Player] > 0;
    }

    public bool IsBattleOngoing()
    {
        return UnitManager.Instance.TeamAliveCount[TeamType.Player] > 0 && UnitManager.Instance.TeamAliveCount[TeamType.Enemy] > 0;
    }
}
