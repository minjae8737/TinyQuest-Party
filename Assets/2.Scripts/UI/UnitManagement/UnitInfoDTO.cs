using UnityEngine;

public class UnitInfoDTO
{
    public Unit Unit { get; set; }
    public UnitName UnitName { get; set; }
    public UnitClass UnitClass { get; set; }
    public Sprite UnitSprite { get; set; }
    public UnitStat Stat { get; set; }
    public int UnitLevel { get; set; }
    public int UnitMaxLevel { get; set; }
    public int StarGrade { get; set; }
    public UnitGradeType UnitGradeType { get; set; }
    
    public UnitInfoDTO()
    {
        UnitName = UnitName.None;
        UnitLevel = 1;
        StarGrade = 1;
    }

    public void SetValue(UnitController unitController)
    {
        PlayerUnitData data = unitController.Model.Data as PlayerUnitData;

        Unit = unitController.Model;
        
        UnitName = data.UnitName;
        UnitClass = data.UnitClass;
        UnitSprite = data.Icon;
        Stat = Unit.Stat;
        StarGrade = Unit.Grade.StarGrade;
        UnitGradeType = Unit.Grade.UnitGradeType;
        UnitLevel = Unit.Level.Level;
        UnitMaxLevel = Unit.Level.MaxLevel;
    }
}
