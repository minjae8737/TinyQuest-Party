using UnityEngine;

[CreateAssetMenu(fileName = "Unit_",menuName = "Unit/UnitData")]
public class UnitData : ScriptableObject
{
    public UnitName UnitName;
    public Stat BaseStat;
}
