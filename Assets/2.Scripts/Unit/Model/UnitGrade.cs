using System;
using UnityEngine;

public enum UnitGradeType
{
    R,    // 흰,회
    SR,   // 노랑,주황, 금
    SSR,  // 무지개
}

[Serializable]
public class UnitGrade
{
    private static readonly int[] MaxFragments = { 10, 30, 50, 100 };
    private static readonly int MaxStarGrade = 5;
    private static readonly int MaxFragmentStack = 9999;
    
    [SerializeField, ReadOnly] private UnitGradeType unitGradeType;
    [SerializeField, ReadOnly] private int starGrade;
    [SerializeField, ReadOnly] private int fragments;
    
    #region Property

    public UnitGradeType UnitGradeType => unitGradeType;

    public int StarGrade
    {
        get => starGrade;
        set
        {
            starGrade = Math.Clamp(value, 1, 5);
        }
    }

    public int Fragments
    {
        get => fragments;
        private set
        {
            fragments = Mathf.Min(value, MaxFragmentStack);
        }
    }

    public bool CanPromote => starGrade < MaxStarGrade && MaxFragments[StarGrade - 1] <= Fragments;

    #endregion

    public event Action OnStarGradeChanged;

    public void Init(UnitData data)
    {
        if (data is PlayerUnitData playerUnitData)
        {
            unitGradeType = playerUnitData.UnitGradeType;
            StarGrade = (int)unitGradeType + 1;
            Fragments = 0;
        }
    }

    public void Promote()
    {
        if (!CanPromote) return;

        Fragments -= MaxFragments[StarGrade - 1];
        StarGrade++;
        
        OnStarGradeChanged?.Invoke();
    }

    public void AddFragments(int amount)
    {
        Fragments += amount;
    }


    public void ApplySaveData(int starGrade, int fragments)
    {
        StarGrade = Mathf.Max(StarGrade, starGrade);
        Fragments = fragments;
    }
}
