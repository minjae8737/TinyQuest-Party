using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private SaveData saveData;

    private async void Start()
    {
        bool isSuccess = await FirebaseAuthManager.Instance.InitAndSignIn();

        if (!isSuccess)
        {
            // 재시도 로직
            // 재시도 UI팝업 재생
            // 확인누르면 씬 재로드
            return;
        }
        
        string uid = FirebaseAuthManager.Instance.CurrentUser?.UserId;

        saveData = await FirestoreManager.Instance.LoadPlayerData(uid);
        
        AudioManager.Instance.Init();

        MapManager.Instance.Init();
        PoolManager.Instance.Init();
        TrainingManager.Instance.Init(saveData.TrainingSaveData);
        UnitManager.Instance.Init(saveData.UnitSaveDatas, saveData.PartySaveData);
        StageManager.Instance.Init(saveData.StageSaveData);
        CurrencyManager.Instance.Init(saveData.CurrencySaveData);
        QuestManager.Instance.Init(saveData.QuestSaveData);

        UIManager.Instance.Init();

        GameStart();
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    #region DataSave

    private async Task<bool> Save()
    {
        saveData.CurrencySaveData = CurrencyManager.Instance.GetCurrencySaveData();
        saveData.UnitSaveDatas = UnitManager.Instance.GetUnitSaveDatas();
        saveData.PartySaveData = UnitManager.Instance.GetPartySaveData();
        saveData.StageSaveData = StageManager.Instance.GetPartySaveData();
        saveData.TrainingSaveData = TrainingManager.Instance.GetSaveData();
        saveData.QuestSaveData = QuestManager.Instance.GetQuestSaveData();

        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);

        await FirestoreManager.Instance.SavePlayerData(saveData);
        
        Debug.Log($"SaveData = \n{json}");
        return true;
    }

    #endregion

    #region Stage

    private void GameStart()
    {
        StageManager.Instance.StartStage();
    }

    #endregion

    #region DroppedItem

    /// 스테이지 레벨에 따른 경험치와 골드 보상
    public void DropReward(RewardData reward, Vector3 unitPos)
    {
        if (reward.Gold > 0)
        {
            CurrencyData currencyData = CurrencyManager.Instance.DataDic[CurrencyType.Gold];

            var data = new DroppedItem_Currency()
            {
                Icon = currencyData.Icon,
                Type = CurrencyType.Gold,
                Amount = reward.Gold
            };

            DroppedItem droppedItem = PoolManager.Instance.Get<DroppedItem>();
            droppedItem.Init(data);
            droppedItem.transform.position = unitPos;
        }

        if (reward.Exp > 0)
        {
            CurrencyData currencyData = CurrencyManager.Instance.DataDic[CurrencyType.Exp];

            var data = new DroppedItem_Currency()
            {
                Icon = currencyData.Icon,
                Type = CurrencyType.Exp,
                Amount = reward.Exp
            };

            DroppedItem droppedItem = PoolManager.Instance.Get<DroppedItem>();
            droppedItem.Init(data);
            droppedItem.transform.position = unitPos;
        }

        //TODO 장비 아이템 추가시 로직 채우기
        foreach (Item rewardItem in reward.Items)
        {
            // Item data받아오기

            var data = new DroppedItem_Item()
            {
                // Item 데이터 할당
                // item = 
            };

            DroppedItem droppedItem = PoolManager.Instance.Get<DroppedItem>();
            droppedItem.Init(data);
            droppedItem.transform.position = unitPos;
        }
    }

    #endregion
}