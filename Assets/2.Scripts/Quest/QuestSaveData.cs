using System.Collections.Generic;
using Firebase.Firestore;

[FirestoreData]
public class QuestSaveData
{
    [FirestoreProperty]
    public int mainQuestIdx { get; set; }
    
    [FirestoreProperty]
    public Dictionary<string, long> counter { get; set; }

    public QuestSaveData() { }

    public QuestSaveData(int curMainQuestIdx, Dictionary<string, long> savedCounter)
    {
        mainQuestIdx = curMainQuestIdx;
        counter = savedCounter;
    }

    public static QuestSaveData Create()
    {
        return new QuestSaveData(
            curMainQuestIdx: 0,
            savedCounter: new()
        );
    }
}
