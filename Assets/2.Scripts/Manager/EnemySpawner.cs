using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }
    
    [SerializeField] private Vector2 maxOffset;
    [SerializeField] private Vector2 minOffset;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Spawn(UnitName spawnUnitName, Vector2 spawnPos)
    {
        float randRangeX = Random.Range(-0.5f, 0.5f);

        Vector2 randPos = new Vector2(randRangeX, 0f) + spawnPos;
        
        UnitManager.Instance.Spawn(spawnUnitName, randPos);
    }

    public void Spawn(WavePattern pattern, Vector2 spawnPos)
    {
        foreach (EnemyWave wave in pattern.waves)
        {
            for (int i = 0; i < wave.SpawnCount; i++)
            {
                float randRangeX = Random.Range(-0.5f, 0.5f);
                Vector2 randPos = new Vector2(randRangeX, 0f) + spawnPos;
                UnitManager.Instance.Spawn(wave.UnitName, randPos);
            }
        }
    }
}