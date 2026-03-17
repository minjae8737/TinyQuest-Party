using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    [SerializeField] private Transform MapParent;
    
    private int curstageIdx = 0;
    private int[] islandXPos = { 0, 20, 40, 60 };
    
    private List<Island> islands = new();
    [SerializeField] private List<GameObject> islandPrefabs;

    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void LoadIsland(StageData stageData)
    {
        //TODO 생성된 오브젝트 처리하기 -> pooling 하거나 destroy 
        islands.Clear();

        for (int i = 0; i < stageData.IslandDatas.Count; i++)
        {
            int randomIdx = Random.Range(0, islandPrefabs.Count);
            
            GameObject islandObj = Instantiate(islandPrefabs[randomIdx], MapParent);
            if(!islandObj.TryGetComponent<Island>(out var island)) continue;
            
            islands.Add(island);
            island.transform.position = new Vector2(islandXPos[i], 0f);
        }
    }

    public GameObject GetCurIsland(int curIslandIdx)
    {
        return islands[curIslandIdx].gameObject;
    }
    
    public Vector2 GetCurIslandPos(int curIslandIdx)
    {
        return islands[curIslandIdx].transform.position;
    }

    public Vector2 GetEnemySpawnPos(int curIslandIdx)
    {
        return GetCurIslandPos(curIslandIdx) + islands[curIslandIdx].EnemySpawnPos;
    }

    public Vector2 GetPlayerSpawnPos(int curIslandIdx)
    {
        return GetCurIslandPos(curIslandIdx) + islands[curIslandIdx].PlayerSpawnPos;
    }
}