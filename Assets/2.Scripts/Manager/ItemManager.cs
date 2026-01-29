using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    private Dictionary<long, Item> items;
    private Dictionary<string, ItemData> itemDatas;

    private static long itemId = 1L;

    [SerializeField] private List<ArmorData> armorDatas;
    [SerializeField] private List<WeaponData> weaponDatas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        items = new Dictionary<long, Item>();
        itemDatas = new Dictionary<string, ItemData>();

        foreach (ItemData itemData in armorDatas)
        {
            if (!itemDatas.TryAdd(itemData.DataId, itemData))
            {
                Debug.Log(itemData.DataId + " Item Data Id is duplicated");
            }
        }

        foreach (ItemData itemData in weaponDatas)
        {
            if (!itemDatas.TryAdd(itemData.DataId, itemData))
            {
                Debug.Log(itemData.DataId + " Item Data Id is duplicated");
            }
        }
    }

    public Item Get(long itemId)
    {
        if (!items.TryGetValue(itemId, out var item))
        {
            Debug.LogError(itemId + " item is Null");
        }

        return item;
    }

    public ItemData GetData(string dataId)
    {
        if (!itemDatas.TryGetValue(dataId, out var itemData))
        {
            Debug.LogError(dataId + " ItemData is Null");
        }

        return itemData;
    }

    public Item CreateItem(ItemData itemData)
    {
        Item newItem = new Item(itemId, itemData.DataId);
        
        if (!items.TryAdd(itemId, newItem))
        {
            Debug.LogError(itemId + " ItemData is duplicated");
        }

        itemId++;
        return newItem;
    }
}