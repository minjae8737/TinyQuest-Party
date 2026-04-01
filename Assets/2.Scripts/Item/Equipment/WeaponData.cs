using UnityEngine;

[CreateAssetMenu(fileName = "Item_", menuName = "Item/Equipment/Weapon")]
public class WeaponData : EquipmentData
{
    public WeaponType Type;
    public bool IsTwoHanded;
    
    private void OnValidate()
    {
        if (DataId == string.Empty)
        {
            string className = "Weapon";
            string weaponTypeName = Type.ToString();
            string dataId = string.Format("{0}_{1}_00", className, weaponTypeName);
            DataId = dataId;
        }
    }
}
