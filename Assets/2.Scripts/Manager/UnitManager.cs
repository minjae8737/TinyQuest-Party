using System;
using System.Collections.Generic;
using UnityEngine;

public enum UnitName
{
    None = -1,
    Archer,
    Armored_Orc,
    Knight,
    Swordsman,
    Knight_Templar,
    Armored_Axeman,
    Soldier,
    Wizard,
    Priest,
    Armored_Skeleton,
    Elite_Orc,
    Greatsword_Skeleton,
    Orc,
    Orc_rider,
    Skeleton,
    Skeleton_Archer,
    Slime,
    Werebear,
    Werewolf,
    Lancer,
}

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }
    public Dictionary<UnitName, UnitData> UnitDataDic { get; private set; }
    private Dictionary<UnitName, GameObject> unitPrefabDic;
    private Dictionary<UnitName, List<UnitController>> unitPoolsDic;
    public Dictionary<TeamType, List<UnitController>> TeamUnitDic { get; private set; }
    public Dictionary<TeamType, int> TeamAliveCount { get; private set; }
    public Dictionary<TeamType, List<UnitName>> UnitNamesByTeam { get; private set; }
    public List<UnitController> Units { get; private set; }
    private Stack<SkillEffect> skillEffectStack;

    private Party party;
    public const int MaxPartySize = 4;
    
    [SerializeField] private Transform playerGroupTransform;
    [SerializeField] private Transform enemyGroupTransform;

    [Header("=== Datas ===")] 
    [SerializeField] private List<UnitData> datas;
    
    [Header("=== Unit Prefabs ===")]
    [SerializeField] private List<GameObject> unitPrefabs;
    
    [Header("=== Unit HP Bar ===")]
    [SerializeField] private RectTransform unitHpBarParent;
    
    [Header("=== Effect Prefabs ===")]
    [SerializeField] private GameObject skillEffectPrefab;

    
    #region Event

    public event Action OnPartyChanged;

    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Init()
    {
        UnitDataDic = new Dictionary<UnitName, UnitData>();
        unitPrefabDic = new Dictionary<UnitName, GameObject>();
        unitPoolsDic = new Dictionary<UnitName, List<UnitController>>();
        TeamUnitDic = new Dictionary<TeamType, List<UnitController>>();
        TeamAliveCount = new Dictionary<TeamType, int>();
        UnitNamesByTeam = new Dictionary<TeamType, List<UnitName>>();
        Units = new List<UnitController>();
        skillEffectStack = new Stack<SkillEffect>();
        
        TeamUnitDic.Add(TeamType.Player,new());
        TeamUnitDic.Add(TeamType.Enemy,new());
        TeamAliveCount.Add(TeamType.Player, 0);
        TeamAliveCount.Add(TeamType.Enemy, 0);
        UnitNamesByTeam.Add(TeamType.Player, new());
        UnitNamesByTeam.Add(TeamType.Enemy, new());

        // UnitDataDic 초기화
        foreach (UnitData unitData in datas)
        {
            UnitDataDic.Add(unitData.UnitName, unitData);
        }
        
        // Prefab 등록
        foreach (GameObject prefab in unitPrefabs)
        {
            if (!prefab.TryGetComponent<UnitController>(out var unitController))
            {
                Debug.LogError(prefab.name + " prefab is don`t have UnitController");
                continue;
            }

            UnitName unitName = unitController.Model.Data.UnitName;
            unitPrefabDic.Add(unitName, prefab);
            UnitNamesByTeam[unitController.TeamType].Add(unitName);
        }
        
      
        
        //TODO test code 파티 초기화
        party = new();
        AssignUnitToSlot(0,UnitName.Wizard);
        AssignUnitToSlot(1,UnitName.Priest);
        AssignUnitToSlot(2,UnitName.Swordsman);
        AssignUnitToSlot(3,UnitName.Knight);
    }

    #region Unit
    
    public UnitController CreateUnit(UnitName unitName)
    {
        if (!unitPrefabDic.TryGetValue(unitName, out var prefab))
        {
            Debug.LogError("UnitName : " + unitName + " is Null");
        }
        
        GameObject newUnit = Instantiate(prefab);
        newUnit.SetActive(false);
        newUnit.TryGetComponent<UnitController>(out var unitController);
        newUnit.transform.parent = unitController.TeamType == TeamType.Player ? playerGroupTransform : enemyGroupTransform;

        if (!unitPoolsDic.TryGetValue(unitName, out List<UnitController> pool))
        {
            if (pool == null)
            {
                pool = new List<UnitController>();
                unitPoolsDic.Add(unitName, pool);
            }
        }

        pool.Add(unitController);
        TeamUnitDic[unitController.TeamType].Add(unitController);
        Units.Add(unitController);
        return unitController;
    }

    public void Spawn(UnitName unitName , Vector2 spawnPos)
    {
        if (unitName == UnitName.None) return;
        
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
        TeamAliveCount[unitController.TeamType]++;
    }

    public void Despawn(UnitController unitController)
    {
        unitController.gameObject.SetActive(false);
        TeamAliveCount[unitController.TeamType]--;
    }

    public void DespawnPlayerParty()
    {
        List<UnitController> playerTeam = TeamUnitDic[TeamType.Player];

        foreach (UnitController controller in playerTeam)
        {
            if (controller.gameObject.activeSelf)
            {
                controller.Despawn();
            }
        }
    }
    
    public UnitHpBar GetUnitHpBar()
    {
        UnitHpBar hpBar = PoolManager.Instance.Get<UnitHpBar>();
        if (hpBar == null) return null;
        
        return hpBar;
    }

    public void ReleaseUnitHpBar(UnitHpBar hpBar)
    {
        PoolManager.Instance.Release(hpBar);
    }
    
    public void CombatEnabled(bool enabled)
    {
        List<UnitController> playerTeam = TeamUnitDic[TeamType.Player];
        List<UnitController> enemyTeam = TeamUnitDic[TeamType.Enemy];
        
        foreach (UnitController controller in playerTeam)
        {
            if (controller.gameObject.activeSelf && controller.TryGetComponent<AutoCombat>(out var autoCombat))
            {
                autoCombat.SetEnabled(enabled);
            }
        }
        
        foreach (UnitController controller in enemyTeam)
        {
            if (controller.gameObject.activeSelf && controller.TryGetComponent<AutoCombat>(out var autoCombat))
            {
                autoCombat.SetEnabled(enabled);
            }
        }
    }
    
    #endregion

    #region Party

    public void SpawnParty(Vector2 spawnPos)
    {
        foreach (PartySlot slot in party.Slots)
        {
            Spawn(slot.UnitName, spawnPos);
        }
    }
    
    public void AssignUnitToSlot(int slotIdx, UnitName unitName)
    {
        int originIdx = party.FindUnitSlotIndex(unitName);
        
        if (originIdx == slotIdx) return;

        if (party.HasUnit(unitName))
        {
            party.SwapPartySlot(originIdx, slotIdx);
        }
        else
        {
            party.SetSlot(slotIdx, unitName);
        }

        OnPartyChanged?.Invoke();
    }

    public void RemoveUnit(UnitName unitName)
    {
        int slotIndex = party.FindUnitSlotIndex(unitName);

        if (slotIndex != -1)
        {
            party.SetSlot(slotIndex, UnitName.None);
        }
        
        OnPartyChanged?.Invoke();
    }

    public List<UnitData> GetPartyData()
    {
        List<UnitData> unitDatas = new();

        foreach (PartySlot partySlot in party.Slots)
        {
            UnitDataDic.TryGetValue(partySlot.UnitName, out var unitData);
            unitDatas.Add(unitData);
        }

        return unitDatas;
    }

    #endregion

    #region SkillEffect

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

    #endregion

}