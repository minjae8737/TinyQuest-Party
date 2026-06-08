using System;
using System.Collections.Generic;
using Firebase.Firestore;

[Serializable]
[FirestoreData]
public class UnitSaveData
{
    // UnitLevel
    [FirestoreProperty]
    public UnitName UnitName { get; set; }
    
    [FirestoreProperty]
    public int Level { get; set; }
    
    [FirestoreProperty]
    public long Exp { get; set; }

    [FirestoreProperty]
    public int StarGrade { get; set; }
    
    // UnitEquipment
    // public Dictionary<EquipPart, string> Equipments;

    public UnitSaveData() { }
}
