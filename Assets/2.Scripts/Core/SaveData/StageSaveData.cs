using Firebase.Firestore;

[FirestoreData]
public class StageSaveData
{
    [FirestoreProperty]
    public int CurStageLevel { get; set; }
    
    [FirestoreProperty]
    public int CurIslandIdx { get; set; }

    public StageSaveData() { }

    public StageSaveData(int curStageLevel, int curIslandIdx)
    {
        CurStageLevel = curStageLevel;
        CurIslandIdx = curIslandIdx;
    }

    public static StageSaveData Create()
    {
        return new StageSaveData(
            curStageLevel: 0,
            curIslandIdx: 0
        );
    }
}
