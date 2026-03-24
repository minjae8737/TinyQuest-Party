using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // Json 데이터 로드
        
        MapManager.Instance.Init();
        PoolManager.Instance.Init();
        UnitManager.Instance.Init();
        UIManager.Instance.Init();
        StageManager.Instance.Init();
        CurrencyManager.Instance.Init();
        
        // TODO Manager들 Init()후 시작하도록 수정
        GameStart();
    }

    #region DataSave

    public void SaveData()
    {
        UnitSaveData unitSaveData = new UnitSaveData();

        string path = Path.Combine(Application.persistentDataPath, "player.json");
        string json = JsonConvert.SerializeObject(unitSaveData, Formatting.Indented);
        Debug.Log(json);
        File.WriteAllText(path, json);
    }

    public void LoadSaveData()
    {
        string path = Path.Combine(Application.persistentDataPath, "player.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            UnitSaveData data = JsonConvert.DeserializeObject<UnitSaveData>(json);
        }
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