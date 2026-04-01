using System;
using UnityEngine;

public class ExpCalculator : MonoBehaviour
{
    public static ExpCalculator Instance;

    public ExpTable ExpTable;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public long GetMaxExp(int level)
    {
        ExpData expData = ExpTable.ExpDatas.Find(data =>
            level >= data.MinLevel && (data.MaxLevel == -1 || level <= data.MaxLevel));

        if (expData == null)
        {
            Debug.LogError(level + " level ExpData is null");
        }

        long maxExp = (long)Math.Ceiling(expData.BaseExp * Math.Pow(level, expData.Power));

        return maxExp;
    }
}