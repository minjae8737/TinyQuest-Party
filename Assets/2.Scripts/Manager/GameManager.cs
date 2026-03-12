using System;
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
}