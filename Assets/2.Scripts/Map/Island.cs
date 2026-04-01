using System;
using UnityEngine;

[Serializable]
public class Island : MonoBehaviour
{
    [SerializeField] private Vector2 playerSpawnPos;
    [SerializeField] private Vector2 enemySpawnPos;

    public Vector2 PlayerSpawnPos => playerSpawnPos;
    public Vector2 EnemySpawnPos => enemySpawnPos;
}
