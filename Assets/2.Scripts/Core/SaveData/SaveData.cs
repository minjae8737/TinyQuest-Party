using System.Collections.Generic;

public class SaveData
{
    // 유저 데이터
    public CurrencySaveData CurrencySaveData; 
    
    // Unit 데이터
    public List<UnitSaveData> UnitSaveDatas = new();
    
    // 파티 데이터
    public PartySaveData PartySaveData;
    
    // 스테이지 데이터
    public StageSaveData StageSaveData;
    
    // Training 데이터
    public TrainingSaveData TrainingSaveData;
}
