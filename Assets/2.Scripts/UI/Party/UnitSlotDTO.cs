public class UnitSlotDTO
{
    public UnitName UnitName { get; set; }
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

}
