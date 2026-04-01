using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExpTable", menuName = "Exp/ExpTable")]
public class ExpTable : ScriptableObject
{
    [SerializeField] public List<ExpData> ExpDatas;
}
