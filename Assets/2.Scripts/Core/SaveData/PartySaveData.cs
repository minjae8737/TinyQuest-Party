using System.Collections.Generic;

public class PartySaveData
{
    public List<UnitName> UnitList = new();

    public PartySaveData(List<UnitName> unitList)
    {
        UnitList = unitList;
    }
}