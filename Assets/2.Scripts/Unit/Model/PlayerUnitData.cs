using UnityEngine;

[CreateAssetMenu(fileName = "Player_",menuName = "Unit/Data/Player")]
public class PlayerUnitData : UnitData
{
    public override TeamType TeamType => TeamType.Player;
    
    public UnitClass UnitClass;
    [Range(1,5)] public int StartStarGrade;
    

}
