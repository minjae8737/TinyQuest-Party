using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public SaveData SaveData { get; private set; }
    public string UserId => SaveData.UserId;

    protected override void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private async void Start()
    {
        // bool isSuccess = await FirebaseAuthManager.Instance.InitAndSignIn();
        //
        // if (!isSuccess)
        // {
        //     // 재시도 로직
        //     // 재시도 UI팝업 재생
        //     // 확인누르면 씬 재로드
        //     return;
        // }
        //
        // string uid = FirebaseAuthManager.Instance.CurrentUser?.UserId;
        //
        // await FirestoreManager.Instance.Init();
        // await FirebaseFunctionsManager.Instance.Init();
        //
        // SaveData = await FirestoreManager.Instance.LoadPlayerData(uid);
        //
        // AudioManager.Instance.Init();
        //
        // MapManager.Instance.Init();
        // PoolManager.Instance.Init();
        // TrainingManager.Instance.Init(SaveData.TrainingSaveData);
        // UnitManager.Instance.Init(SaveData.UnitSaveDatas, SaveData.PartySaveData);
        // StageManager.Instance.Init(SaveData.StageSaveData);
        // CurrencyManager.Instance.Init(SaveData.CurrencySaveData);
        // QuestManager.Instance.Init(SaveData.QuestSaveData);
        // GachaManager.Instance.Init(SaveData.PityCount);
        //
        // UIManager.Instance.Init();
        //
        // GameStart();
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    #region DataSave

    private async Task<bool> Save()
    {
        SaveData.CurrencySaveData = CurrencyManager.Instance.GetCurrencySaveData();
        SaveData.UnitSaveDatas = UnitManager.Instance.GetUnitSaveDatas();
        SaveData.PartySaveData = UnitManager.Instance.GetPartySaveData();
        SaveData.StageSaveData = StageManager.Instance.GetStageSaveData();
        SaveData.TrainingSaveData = TrainingManager.Instance.GetSaveData();
        SaveData.QuestSaveData = QuestManager.Instance.GetQuestSaveData();
        SaveData.PityCount = GachaManager.Instance.PityCount;

        string json = JsonConvert.SerializeObject(SaveData, Formatting.Indented);

        await FirestoreManager.Instance.SavePlayerData(SaveData);
        
        Debug.Log($"SaveData = \n{json}");
        return true;
    }
    
    public void SetSaveData(SaveData data)
    {
        SaveData = data;
    }

    #endregion

    #region Stage

    public void GameStart()
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

            DroppedItem droppedItem = BattlePoolManager.Instance.Get<DroppedItem>();
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

            DroppedItem droppedItem = BattlePoolManager.Instance.Get<DroppedItem>();
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

            DroppedItem droppedItem = BattlePoolManager.Instance.Get<DroppedItem>();
            droppedItem.Init(data);
            droppedItem.transform.position = unitPos;
        }
    }

    #endregion
}