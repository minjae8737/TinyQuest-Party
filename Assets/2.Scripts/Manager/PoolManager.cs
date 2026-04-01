using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }
    
    private Dictionary<Type, Stack<Poolable>> poolDic;
    private Dictionary<Type, GameObject> prefabDic;
    private Dictionary<Type, Transform> parentDic;

    [Header("=== Prefabs ===")]
    [SerializeField] private List<ObjPrefab> prefabs;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Init()
    {
        poolDic = new();
        prefabDic = new();
        parentDic = new();
        
        foreach (ObjPrefab objPrefab in prefabs)
        {
            if (objPrefab.Prefab == null)
            {
                Debug.LogError("Prefab is null");
                continue;
            }

            Poolable poolable = objPrefab.Prefab.GetComponent<Poolable>();

            if (poolable == null)
            {
                Debug.LogError($"{objPrefab.Prefab.name} : Poolable 없음");
                continue;
            }

            Type type = poolable.GetType();
            
            prefabDic.Add(type, objPrefab.Prefab);
            parentDic.Add(type, objPrefab.parent);
        }
    }

    private T Create<T>() where T : Poolable
    {
        Type type = typeof(T);

        if (!poolDic.TryGetValue(type, out var pool))
        {
            poolDic.Add(type, new Stack<Poolable>());
            pool = poolDic[type];
        }

        if (!prefabDic.TryGetValue(type, out var prefab))
        {
            Debug.LogError($"Not Found {type} prefab." );
            return null;
        }
        
        Transform parent = parentDic[type];

        GameObject newObj = Instantiate(prefab, parent);
        var component = newObj.GetComponent<T>();

        return component;
    }
    
    public T Get<T>() where T : Poolable
    {
        Type type = typeof(T);

        if (!poolDic.TryGetValue(type, out var pool) || pool.Count == 0)
        {
            T newObj = Create<T>();
            newObj.gameObject.SetActive(true);
            return newObj;
        }

        Component obj = pool.Pop();
        obj.gameObject.SetActive(true);
        
        return (T)obj;
    }

    public void Release(Poolable obj)
    {
        Type type = obj.GetType();
        obj.gameObject.SetActive(false);
        
        if (!poolDic.TryGetValue(type, out var pool))
        {
            Debug.LogError($"Release Fail. " + type + " not registered in PoolManager.");
            return;
        }
        
        poolDic[type].Push(obj);
    }
}