using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    //TODO : FIXME 하드코딩 키값
    private const string KEY = "Xq3#mK9@pL2$vN7!rT5&wB8^zC4*jF62"; // 서버에서 키를 가져오는것이 안전 
    private const string IV = "Hd6!yM2@kP9#qR4$"; // 서버에서 키를 가져오는것이 안전

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
        TrainingManager.Instance.Init(saveData.TrainingSaveData);
        UnitManager.Instance.Init(saveData.PartySaveData);
        StageManager.Instance.Init(saveData.StageSaveData);
        CurrencyManager.Instance.Init(saveData.CurrencySaveData);
        QuestManager.Instance.Init();

        UIManager.Instance.Init();

        GameStart();
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    #region DataSave

    private bool Save()
    {
        SaveData saveData = new SaveData();

        saveData.CurrencySaveData = CurrencyManager.Instance.GetCurrencySaveData();
        saveData.PartySaveData = UnitManager.Instance.GetPartySaveData();
        saveData.StageSaveData = StageManager.Instance.GetPartySaveData();
        saveData.TrainingSaveData = TrainingManager.Instance.GetSaveData();

        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
        string cipherText = Encrypt(json);

        try
        {
            File.WriteAllText(savePath, cipherText);
        }
        catch (Exception e)
        {
            throw new IOException("Fail Write SaveData File.");
        }

        return true;
    }

    private bool Load()
    {
        try
        {
            if (File.Exists(savePath))
            {
                string cipherText = File.ReadAllText(savePath);
                string plainText = Decrypt(cipherText);

                saveData = JsonConvert.DeserializeObject<SaveData>(plainText);
            }
        }
        catch (Exception e)
        {
            throw new FileLoadException("Fail Load SaveData File.");
        }

        return true;
    }

    private string Encrypt(string plainText)
    {
        using Aes aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(KEY); // KEY 문자열을 UTF8 바이트 배열로 변환
        aes.IV = Encoding.UTF8.GetBytes(IV); // 같은 데이터를 암호화해도 매번 다른 결과가 나오게 해주는 값
        aes.Mode = CipherMode.CBC; // CBC = 이전 블록의 암호화 결과가 다음 블록에 영향을 줌
        aes.Padding = PaddingMode.PKCS7; // 데이터 빈 공간을 채우는 방식 지정

        using var encryptor = aes.CreateEncryptor(); // 설정된 Key + IV로 실제 암호화를 수행할 Encryptor 객체 생성
        using var ms = new MemoryStream(); // 암호화된 결과 바이트를 담을 메모리 버퍼 생성(메모리에 임시 저장)
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write); // 데이터를 쓸 때 자동으로 암호화해주는 스트림, cs에 쓰면 → encryptor가 암호화 → ms(메모리)에 저장되는 파이프라인
        using var sw = new StreamWriter(cs); // 문자열을 스트림에 편하게 쓰기 위한 Writer, sw에 문자열 쓰면 → cs(암호화) → ms(메모리) 순

        sw.Write(plainText); // 평문을 파이프라인에 흘려보냄.
        sw.Close(); // 버퍼에 남은 데이터까지 강제로 flush
        
        return Convert.ToBase64String(ms.ToArray()); // ms에 담긴 암호화된 바이트 배열을 Base64 문자열로 변환해서 문자열로 인코딩
    }

    private string Decrypt(string cipherText)
    {
        using Aes aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(KEY);
        aes.IV = Encoding.UTF8.GetBytes(IV);
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        var buffer = Convert.FromBase64String(cipherText); // Encrypt에서 Base64로 인코딩했던 걸 다시 byte[]로 디코딩

        using var decryptor = aes.CreateDecryptor(); // 복호화를 수행할 Decryptor 객체 생성
        using var ms = new MemoryStream(buffer); // 복호화할 바이트 배열(buffer)을 메모리 스트림에 로드
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read); // ms에서 읽을 때 자동으로 복호화해주는 스트림, cs에서 읽으면 → decryptor가 복호화 → 원본 데이터
        using var sr = new StreamReader(cs); // 복호화된 바이트를 문자열로 읽기 위한 Reader
        
        return sr.ReadToEnd();; // 복호화된 전체 내용을 문자열로 읽어서 반환
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