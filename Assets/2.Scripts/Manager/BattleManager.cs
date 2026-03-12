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

    private void FixedUpdate()
    {
        if (!isBattle) return;
        
        if (Time.time - lastTime >= delay)
        {
            CheckBattleEnd();
            lastTime = Time.time;
        }
    }

    public void BattleStart()
    {
        UnitManager.Instance.CombatEnabled(true);
        isBattle = true;
    }

    public void BattlePause()
    {
        UnitManager.Instance.CombatEnabled(false);
        isBattle = false;
    }

    public void CheckBattleEnd()
    {
        if (IsBattleOngoing()) return;

        BattleEnd();
    }

    public void BattleEnd()
    {
        BattlePause();
        
        UnitManager.Instance.DespawnPlayerParty();
        StageManager.Instance.OnIslandClear();
        // StageManager.Instance.OnIslandFail(); // 성공여부에따른 분기
    }
    
    public bool IsBattleOngoing()
    {
        return UnitManager.Instance.TeamAliveCount[TeamType.Player] > 0 && UnitManager.Instance.TeamAliveCount[TeamType.Enemy] > 0;
    }
}
