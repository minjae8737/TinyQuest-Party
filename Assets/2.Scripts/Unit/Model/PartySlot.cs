using System;

[Serializable]
public class PartySlot
{
    public UnitName UnitName;

    public bool IsEmpty()
    {
        return UnitName == UnitName.None;
    }
}
