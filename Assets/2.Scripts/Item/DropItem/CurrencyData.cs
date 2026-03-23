using UnityEngine;

[CreateAssetMenu(fileName = "Currency_", menuName = "Currency")]
public class CurrencyData : ScriptableObject
{
    public string DataId;
    public string Name;
    public Sprite Icon;
    public CurrencyType Type;
}
