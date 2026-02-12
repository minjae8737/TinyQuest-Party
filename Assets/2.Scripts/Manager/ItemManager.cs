using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    private static long itemId = 1L;
    
    private Dictionary<long, Item> itemDic;
    private Dictionary<string, ItemData> itemDataDic;

    [Header("=== Item Data Tables ===")]
    [SerializeField] private List<ArmorData> armorDatas;
    [SerializeField] private List<WeaponData> weaponDatas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        itemDic = new Dictionary<long, Item>();
        itemDataDic = new Dictionary<string, ItemData>();

        foreach (ItemData itemData in armorDatas)
        {
            if (!itemDataDic.TryAdd(itemData.DataId, itemData))
            {
                Debug.Log(itemData.DataId + " Item Data Id is duplicated");
            }
        }

        foreach (ItemData itemData in weaponDatas)
        {
            if (!itemDataDic.TryAdd(itemData.DataId, itemData))
            {
                Debug.Log(itemData.DataId + " Item Data Id is duplicated");
            }
        }
    }

    public Item Get(long itemId)
    {
        if (!itemDic.TryGetValue(itemId, out var item))
        {
            Debug.LogError(itemId + " item is Null");
        }

        return item;
    }

    public ItemData GetData(string dataId)
    {
        if (!itemDataDic.TryGetValue(dataId, out var itemData))
        {
            Debug.LogError(dataId + " ItemData is Null");
        }

        return itemData;
    }

    public Item CreateItem(ItemData itemData)
    {
        Item newItem = new Item(itemId, itemData.DataId);
        
        if (!itemDic.TryAdd(itemId, newItem))
        {
            Debug.LogError(itemId + " ItemData is duplicated");
        }

        itemId++;
        return newItem;
    }
}