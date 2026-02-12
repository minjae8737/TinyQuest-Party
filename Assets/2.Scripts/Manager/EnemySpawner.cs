using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public Transform playerTr;

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
        Vector2 randPos = new Vector2(Random.Range(4.5f, 5.5f) * (Random.value < 0.5f ? 1 : -1),
            Random.Range(9f, 11f) * (Random.value < 0.5f ? 1 : -1));
        // Debug.Log(randPos);
        UnitManager.Instance.Spawn(UnitName.Armored_Orc, (Vector2)playerTr.position + randPos);
    }
}