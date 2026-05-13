using UnityEngine;

public static class UnitStatCalculator
{
    private const float LevelMultiplier = 1.05f;
    private static readonly float[] StarMultiplier = { 1.0f, 1.25f, 1.6f, 2.1f, 2.8f };
    private static Stat zeroStat = new Stat();
    

    public static Stat GetBaseStat(UnitData data, int level = 1, int starGrade = 1)
    {
        Stat baseStat = data.BaseStat.Clone();

        if (data.TeamType == TeamType.Player)
        {
            return baseStat * GetLevelMultiplier(level) * GetStarMultiplier(starGrade);
        }
        else
        {
            return baseStat *= StageManager.Instance.GetStageScaling();
        }
    }

    public static Stat GetTrainingStat(UnitData data)
    {
        if (data.TeamType == TeamType.Player)
        {
            return TrainingManager.Instance.TotalStat;
        }
        else
        {
            return zeroStat;
        }
    }

    private static float GetLevelMultiplier(int level)
    {
        return Mathf.Pow(LevelMultiplier, level - 1);
    }
    
    private static float GetStarMultiplier(int starGrade)
    {
        return StarMultiplier[starGrade - 1];
    }
}
