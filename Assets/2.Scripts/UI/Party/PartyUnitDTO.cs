public class PartyUnitDTO
{
    public UnitName UnitName;
    public UnitData Data;
    public int unitLevel;
    public int starGrade;

    public PartyUnitDTO()
    {
        UnitName = UnitName.None;
        Data = null;
        unitLevel = 1;
        starGrade = 1;
    }
}
