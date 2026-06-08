using System;
using System.Threading.Tasks;
using Firebase.Firestore;
using UnityEngine;

public class FirestoreManager : Singleton<FirestoreManager>
{
    private FirebaseFirestore db;

    private void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
    }
    
    // 저장
    public async Task SavePlayerData(SaveData data)
    {
        DocumentReference docRef = db
            .Collection("players")
            .Document(data.UserId);

        await docRef.SetAsync(data);
        Debug.Log("저장 완료");
    }

    // 불러오기
    public async Task<SaveData> LoadPlayerData(string userId)
    {
        DocumentReference docRef = db
            .Collection("players")
            .Document(userId);

        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            SaveData data = snapshot.ConvertTo<SaveData>();
            Debug.Log($"불러오기 완료: {data.UserId}");
            return data;
        }
        else
        {
            Debug.Log("신규 유저 — 데이터 생성");
            SaveData newData = new SaveData(userId);
            await SavePlayerData(newData);
            return newData;
        }
    }
}
