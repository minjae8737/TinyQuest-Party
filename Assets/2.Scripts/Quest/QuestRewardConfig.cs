using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/RewardConfig")]
public class QuestRewardConfig : ScriptableObject
{
    [Header("=== Icon ===")]
    [SerializeField] private List<QuestRewardIconData> icons; 
    private Dictionary<RewardType, Sprite> iconMap;
    
    public void Init()
    {
        iconMap = new();
        foreach (var iconData in icons)
        {
            iconMap.TryAdd(iconData.Type, iconData.Sprite);
        }
    }
    
    public Sprite GetIcon(RewardType type)
    {
        iconMap.TryGetValue(type, out Sprite sprite);
        return sprite;
    }
}

[Serializable]
public class QuestRewardIconData
{
    public RewardType Type;
    public Sprite Sprite;
}
