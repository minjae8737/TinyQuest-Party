using UnityEngine;

[CreateAssetMenu(fileName = "Enemy_",menuName = "Unit/Data/Enemy")]
public class EnemyUnitData : UnitData
{
    public override TeamType TeamType => TeamType.Enemy;
}
