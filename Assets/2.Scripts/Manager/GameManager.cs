using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
       
    private string savePath => Path.Combine(Application.persistentDataPath, "playerData.json");
    private SaveData saveData = new();


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
        Load();
        
        AudioManager.Instance.Init();
        
        MapManager.Instance.Init();
        PoolManager.Instance.Init();
        TrainingManaer.Instance.Init(saveData.TrainingSaveData);
        UnitManager.Instance.Init(saveData.PartySaveData);
        StageManager.Instance.Init();
        CurrencyManager.Instance.Init(saveData.CurrencySaveData);
        
        UIManager.Instance.Init();
        
        GameStart();
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    #region DataSave

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TestSaveData();
        }
    }

    public void TestSaveData()
    {
        // SaveData saveData = new SaveData();
        //
        // saveData.CurrencySaveData = CurrencyManager.Instance.GetCurrencySaveData();
        // saveData.PartySaveData = UnitManager.Instance.GetPartySaveData();
        // saveData.TrainingSaveData = TrainingManaer.Instance.GetSaveData();
        //
        // string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
        //
        // Debug.Log(Application.persistentDataPath);
        // Debug.Log($"{json}");
    }
    
    public bool Save()
    {
        SaveData saveData = new SaveData();
        
        saveData.CurrencySaveData = CurrencyManager.Instance.GetCurrencySaveData();
        saveData.PartySaveData = UnitManager.Instance.GetPartySaveData();
        saveData.TrainingSaveData = TrainingManaer.Instance.GetSaveData();

        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
        Debug.Log(Application.persistentDataPath);
        Debug.Log(json);

        try
        {
            File.WriteAllText(savePath, json);
        }
        catch (Exception e)
        {
            throw new IOException("Fail Write SaveData File.");
        }

        return true;
    }

    public bool Load()
    {
        try
        {
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                saveData = JsonConvert.DeserializeObject<SaveData>(json);
            }
        }
        catch (Exception e)
        {
            throw new FileLoadException("Fail Load SaveData File.");
        }
        return true;
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