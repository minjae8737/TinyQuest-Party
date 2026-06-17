using UnityEngine;

public class MainSceneController : MonoBehaviour
{
    private void Start()
    {
        SaveData saveData = GameManager.Instance.SaveData;

        MapManager.Instance.Init();
        BattlePoolManager.Instance.Init();
        TrainingManager.Instance.Init(saveData.TrainingSaveData);
        UnitManager.Instance.Init(saveData.UnitSaveDatas, saveData.PartySaveData);
        StageManager.Instance.Init(saveData.StageSaveData);
        QuestManager.Instance.Init(saveData.QuestSaveData);

        UIManager.Instance.Init();

        GameManager.Instance.GameStart();
    }
}
