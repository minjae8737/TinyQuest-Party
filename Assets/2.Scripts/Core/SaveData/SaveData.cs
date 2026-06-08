using System.Collections.Generic;
using Firebase.Firestore;

[FirestoreData]
public class SaveData
{
    [FirestoreProperty] 
    public string UserId { get; set; } // 유저 아이디
    [FirestoreProperty] 
    public string Nickname { get; set; } // 닉네임

    // 유저 데이터
    [FirestoreProperty] 
    public CurrencySaveData CurrencySaveData { get; set; }

    // Unit 데이터
    [FirestoreProperty] 
    public List<UnitSaveData> UnitSaveDatas { get; set; }

    // 파티 데이터
    [FirestoreProperty] 
    public PartySaveData PartySaveData { get; set; }

    // 스테이지 데이터
    [FirestoreProperty] 
    public StageSaveData StageSaveData { get; set; }

    // Training 데이터
    [FirestoreProperty] 
    public TrainingSaveData TrainingSaveData { get; set; }

    // 퀘스트 데이터
    [FirestoreProperty] 
    public QuestSaveData QuestSaveData { get; set; }

    public SaveData() { }

    public SaveData(string userId)
    {
        UserId = userId;

        CurrencySaveData = CurrencySaveData.Create();
        UnitSaveDatas = new();
        PartySaveData = PartySaveData.Create();
        StageSaveData = StageSaveData.Create();
        TrainingSaveData = TrainingSaveData.Create();
        QuestSaveData = QuestSaveData.Create();
    }
}
