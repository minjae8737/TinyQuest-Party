using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private void Start()
    {
        TestSpawn();
    }

    private void TestSpawn()
    {
        InvokeRepeating("Spawn", 2f, 3f);
    }

    private void Spawn()
    {
        UnitManager.Instance.Spawn(UnitName.Armored_Orc);
    }
}
