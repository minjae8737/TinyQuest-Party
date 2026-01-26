using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public UnitController unitCon;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
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
}
