using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

/// <summary>
/// 게임 진행
/// 1. GameManager 초기화 (데이터, 각 매니저 Init)
/// 2. StagetManager
///     - StartStage()
///         - MapManager에서 스테이지 생성
///         - 섬으로 넘어감
///         -NextIsland()
///             - Player Unit 생성
///             - Enemy Unit 생성
///             
///             - 전투 시작 
///             - 전투 끝
///              
///         
///     
/// </summary>

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
        StageManager.Instance.StageStart();
    }

    #endregion
}