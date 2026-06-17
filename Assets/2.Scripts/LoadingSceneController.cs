using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    private async void Start()
    {
        bool isSuccess = await FirebaseAuthManager.Instance.InitAndSignIn();

        if (!isSuccess)
        {
            // 재시도 로직
            // 재시도 UI팝업 재생
            // 확인누르면 씬 재로드
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }
        
        string uid = FirebaseAuthManager.Instance.CurrentUser?.UserId;
        
        await FirestoreManager.Instance.Init();
        await FirebaseFunctionsManager.Instance.Init();
        
        var saveData = await FirestoreManager.Instance.LoadPlayerData(uid);
        GameManager.Instance.SetSaveData(saveData);
        
        AudioManager.Instance.Init();
        PoolManager.Instance.Init();
        CurrencyManager.Instance.Init(saveData.CurrencySaveData);
        GachaManager.Instance.Init(saveData.PityCount);
        
        Debug.Log("로딩 완료.");
        
        // 다음씬 이동
        SceneManager.LoadScene("2.MainScene");
    }
}
