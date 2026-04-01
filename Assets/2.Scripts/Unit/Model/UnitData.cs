using UnityEngine;

[CreateAssetMenu(fileName = "Unit_",menuName = "Unit/UnitData")]
public class UnitData : ScriptableObject
{
    [Header("Identity")]
    public UnitName UnitName;
    public TeamType TeamType;
    
    [Header("Visual")]
    public Sprite Icon;
    
    [Header("Stats")]
    public Stat BaseStat;
    
    
}
