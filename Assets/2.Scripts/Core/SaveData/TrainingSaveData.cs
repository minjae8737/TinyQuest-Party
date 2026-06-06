using Firebase.Firestore;

[FirestoreData]
public class TrainingSaveData
{
    [FirestoreProperty]
    public int TrainingLevel { get; set; }
    
    [FirestoreProperty]
    public int AttackLevel { get; set; }
    
    [FirestoreProperty]
    public int DefenceLevel { get; set; }
    
    [FirestoreProperty]
    public int HealthLevel { get; set; }

    public TrainingSaveData() { }

    public TrainingSaveData(int trainingLevel, int attackLevel, int defenceLevel, int healthLevel)
    {
        TrainingLevel = trainingLevel;
        AttackLevel = attackLevel;
        DefenceLevel = defenceLevel;
        HealthLevel = healthLevel;
    }

    public static TrainingSaveData Create()
    {
        return new TrainingSaveData(
            trainingLevel: 0,
            attackLevel: 0,
            defenceLevel: 0,
            healthLevel: 0
        );
    }
}