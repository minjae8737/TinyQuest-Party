using UnityEngine;

[CreateAssetMenu(fileName = "Item_", menuName = "Item/Equipment/Armor")]
public class ArmorData : EquipmentData
{
    public ArmorType Type;

    private void OnValidate()
    {
        if (DataId == string.Empty)
        {
            string className = "Armor";
            string armorTypeName = Type.ToString();
            string dataId = string.Format("{0}_{1}_00", className, armorTypeName);
            DataId = dataId;
        }
    }
}
