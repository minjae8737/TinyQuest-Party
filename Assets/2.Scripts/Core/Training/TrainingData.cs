using UnityEngine;

[CreateAssetMenu(fileName = "TrainingLevel_000",menuName = "Training")]
public class TrainingData : ScriptableObject
{
    public int MaxLevel;
    
    public int attackPerLevel;
    public int defencePerLevel;
    public int healthPerLevel;

    public int baseGoldCost;
    public int goldCostPerLevel;
}
