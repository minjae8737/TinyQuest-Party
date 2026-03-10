using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage_00000",menuName = "Stage/Data")]
public class StageData : ScriptableObject
{ 
    public List<IslandData> IslandDatas; 
}
