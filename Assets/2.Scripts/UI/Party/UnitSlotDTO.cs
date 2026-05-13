public class UnitSlotDTO
{
    public UnitName UnitName { get; set; }
    public UnitClass UnitClass { get; set; }
    public UnitData Data { get; set; }
    public int UnitLevel { get; set; }
    public int StarGrade { get; set; }

    public bool HasUnit => UnitName != UnitName.None;

    public UnitSlotDTO()
    {
        UnitName = UnitName.None;
        UnitLevel = 1;
        StarGrade = 1;
    }

    public void SetValue(UnitController unitController)
    {
        PlayerUnitData data = unitController.Model.Data as PlayerUnitData;
        
        UnitName = data.UnitName;
        UnitClass = data.UnitClass;
        Data = data;
        StarGrade = unitController.Model.StarGrade;
        UnitLevel = unitController.Model.Level.Level;
    }

}
