using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public Transform playerTr;
    public UnitName spawnUnitName;
    [SerializeField] private Vector2 maxOffset;
    [SerializeField] private Vector2 minOffset;

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
        // Player 주변 랜덤 범위 설정
        float randRangeX = Random.Range(3.5f, 5.5f);
        // float randRangeY = Random.Range(9f, 11f) * GetRandomSign();

        Vector2 randPos = new Vector2(randRangeX, 0f) + (Vector2)playerTr.transform.position;

        // 맵 너비에 맞게 조정
        // Vector3Int mapSizeVector = MapManager.Instance.mapSizeVector;
        // float posX = Mathf.Clamp(randPos.x, -mapSizeVector.x / 2f + minOffset.x, mapSizeVector.x / 2f + maxOffset.x);
        // float posY = Mathf.Clamp(randPos.y, -mapSizeVector.y / 2f + minOffset.y, mapSizeVector.y / 2f + maxOffset.y);

        Vector2 spawnPos = randPos;

        UnitManager.Instance.Spawn(spawnUnitName, spawnPos);
    }

    // private int GetRandomSign()
    // {
    //     return Random.value < 0.5f ? 1 : -1;
    // }
}