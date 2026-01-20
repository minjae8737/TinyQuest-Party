using System;
using System.Collections.Generic;
using UnityEngine;

public enum ObjType
{
    UnitHpBar,
}

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;
    private Dictionary<ObjType, List<GameObject>> poolDic;
    private Dictionary<ObjType, GameObject> prefabDic;

    [Header("UI")] [SerializeField] private List<ObjPrefab> prefabs;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        poolDic = new Dictionary<ObjType, List<GameObject>>();
        prefabDic = new Dictionary<ObjType, GameObject>();

        for (int i = 0; i < prefabs.Count; i++)
        {
            ObjPrefab objPrefab = prefabs[i];
            if (!prefabDic.ContainsKey(objPrefab.Type) && objPrefab.Prefab != null)
            {
                prefabDic.Add(objPrefab.Type, objPrefab.Prefab);
                poolDic.Add(objPrefab.Type, new List<GameObject>());
            }
        }
    }


    public GameObject Get(ObjType type)
    {
        if (!poolDic.TryGetValue(type, out List<GameObject> pool))
        {
            Debug.LogError("pool is Null. Type : " + type);
        }

        GameObject obj;

        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i] != null && !pool[i].activeSelf)
            {
                obj = pool[i];
                return obj;
            }
        }

        if (!prefabDic.TryGetValue(type, out GameObject prefab))
        {
            Debug.LogError("prefab is Null. Type : " + type);
        }

        obj = Instantiate(prefab);
        pool.Add(obj);

        return obj;
    }
}