using System.Collections.Generic;
using Firebase.Firestore;

[FirestoreData]
public class PartySaveData
{
    [FirestoreProperty]
    public List<UnitName> UnitList { get; set; }

    public PartySaveData() { }

    public PartySaveData(List<UnitName> unitList)
    {
        UnitList = unitList;
    }

    public static PartySaveData Create()
    {
        return new PartySaveData(new());
    }
}