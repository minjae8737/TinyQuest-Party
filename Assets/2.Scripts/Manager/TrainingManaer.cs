using UnityEngine;

public class TrainingManaer : MonoBehaviour
{
    public static TrainingManaer Instance { get; private set; }

    private int trainingLevel; // 
    private int curAttackLevel;
    private int curDefenceLevel;
    private int curHealthLevel;

    private Stat totalStat;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    
}
