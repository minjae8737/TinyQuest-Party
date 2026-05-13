using UnityEngine;

public abstract class UnitData : ScriptableObject
{
    [Header("Visual")]
    public Sprite Icon;
    
    [Header("Identity")]
    public UnitName UnitName;
    public abstract TeamType TeamType { get; }
    
    [Header("Stats")]
    public Stat BaseStat;
}
