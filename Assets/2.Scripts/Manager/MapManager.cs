using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    [Header("=== Reference ===")]
    [SerializeField] private Transform MapParent;
    
    [Header("=== Prefab ===")]
    [SerializeField] private List<GameObject> islandPrefabs;
    
    private int[] islandXPos = { 0, 20, 40, 60 };
    private int islandTypeCount => islandPrefabs.Count;
    
    private List<Island> islands;
    private Dictionary<int, List<Island>> islandPoolsDic;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Init()
    {
        islands = new();
        islandPoolsDic = new Dictionary<int, List<Island>>();
    }

    public void LoadIsland(StageData stageData)
    {
        SetIslandsActive(false);
        islands.Clear();

        for (int i = 0; i < stageData.IslandDatas.Count; i++)
        {
            int islandType = Random.Range(0, islandPrefabs.Count);

            Island island = Get(islandType);
            island.gameObject.SetActive(true);
            
            islands.Add(island);
            float Ypos = Random.Range(-12f, 12f);
            island.transform.position = new Vector2(islandXPos[i], Ypos);
        }
    }

    private Island CreateIsland(int islandType)
    {
        if (islandType >= islandTypeCount)
        {
            Debug.LogError("Create Island Fail. Invalid islandType.");
        }

        GameObject islandObj = Instantiate(islandPrefabs[islandType], MapParent);
        islandObj.SetActive(false);
        islandObj.TryGetComponent<Island>(out var island);
        
        if(!islandPoolsDic.TryGetValue(islandType,out List<Island> pool))
        {
            if (pool == null)
            {
                pool = new List<Island>();
                islandPoolsDic.Add(islandType,pool);
            }
        }

        pool.Add(island);
        return island;
    }

    private Island Get(int islandType)
    {
        if(!islandPoolsDic.TryGetValue(islandType,out List<Island> pool))
        {
            CreateIsland(islandType);
            pool = islandPoolsDic[islandType];
        }

        Island island = null;
        
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].gameObject.activeSelf)
            {
                island = pool[i];
                break;
            }
        }

        if (island == null)
        {
            island = CreateIsland(islandType);
        }

        return island;
    }

    private void SetIslandsActive(bool active)
    {
        foreach (Island island in islands)
        {
            island.gameObject.SetActive(active);
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