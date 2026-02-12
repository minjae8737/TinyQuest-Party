using System;
using System.Collections.Generic;
using UnityEngine;

public enum UnitName
{
    Archer,
    Armored_Orc,
    Knight,
    Swordsman,
    Knight_Templar,
    Armored_Axeman,
    Soldier,
    Wizard
}

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }
    
    private Dictionary<UnitName, GameObject> unitPrefabDic;
    private Dictionary<UnitName, List<UnitController>> unitPoolsDic;
    private Stack<SkillEffect> skillEffectStack;

    [Header("=== Unit Prefabs ===")]
    [SerializeField] private List<GameObject> unitPrefabs;
    
    [Header("=== Effect Prefabs ===")]
    [SerializeField] private GameObject skillEffectPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        unitPrefabDic = new Dictionary<UnitName, GameObject>();
        unitPoolsDic = new Dictionary<UnitName, List<UnitController>>();
        skillEffectStack = new Stack<SkillEffect>();

        foreach (GameObject prefab in unitPrefabs)
        {
            if (!prefab.TryGetComponent<UnitController>(out var unitController))
            {
                Debug.LogError(prefab.name + " prefab is don`t have UnitController");
                continue;
            }

            UnitName unitName = unitController.Model.Data.UnitName;
            unitPrefabDic.Add(unitName, prefab);
        }
    }

    public UnitController CreateUnit(UnitName unitName)
    {
        if (!unitPrefabDic.TryGetValue(unitName, out var prefab))
        {
            Debug.LogError("UnitName : " + unitName + " is Null");
        }

        GameObject newUnit = Instantiate(prefab);
        newUnit.SetActive(false);
        newUnit.TryGetComponent<UnitController>(out var unitController);

        if (!unitPoolsDic.TryGetValue(unitName, out List<UnitController> pool))
        {
            if (pool == null)
            {
                pool = new List<UnitController>();
                unitPoolsDic.Add(unitName, pool);
            }
        }
        
        pool.Add(unitController);
        return unitController;
    }

    public void Spawn(UnitName unitName , Vector2 spawnPos)
    {
        UnitController unitController = null;
        
        if (!unitPoolsDic.TryGetValue(unitName, out List<UnitController> pool))
        {
            CreateUnit(unitName);
            pool = unitPoolsDic[unitName];
        }

        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].gameObject.activeSelf)
            {
                unitController = pool[i];
            }
        }

        if (unitController == null)
        {
            unitController = CreateUnit(unitName);
        }
        
        // Unit 세팅
        unitController.Init();
        unitController.transform.position = spawnPos;
        unitController.gameObject.SetActive(true);
    }

    public void Despawn(UnitController unitController)
    {
        unitController.gameObject.SetActive(false);
    }

    public SkillEffect SpawnSkillEffect(Vector2 targetPos)
    {
        if (!skillEffectStack.TryPop(out SkillEffect skillEffect))
        {
            GameObject skillEffectObj = Instantiate(skillEffectPrefab);
            skillEffect = skillEffectObj.GetComponent<SkillEffect>();
        }
        
        skillEffect.transform.position = targetPos;
        skillEffect.gameObject.SetActive(true);
        return skillEffect;
    }
    
    public void DespawnSkillEffect(SkillEffect skillEffect)
    {
        skillEffect.gameObject.SetActive(false);
        skillEffectStack.Push(skillEffect);
    }
}