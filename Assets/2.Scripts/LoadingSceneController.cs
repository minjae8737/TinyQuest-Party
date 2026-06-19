using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    [SerializeField] private GameObject progressBar;
    [SerializeField] private Image progressFill;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private TMP_Text startText;

    private SaveData saveData;
    private Tween blinkTween;
    private Coroutine getAnyKeyCoroutine;
    
    private async void Start()
    {
        progressBar.SetActive(true);
        startText.gameObject.SetActive(false);
        StartCoroutine(LoadingRoutine());
    }
    
    private void OnDestroy()
    {
        blinkTween?.Kill();
        
        if (getAnyKeyCoroutine != null)
        {
            StopCoroutine(getAnyKeyCoroutine);
            getAnyKeyCoroutine = null;
        }
    }
    
    private IEnumerator LoadingRoutine()
    {
        SetStatus("서버 연결 중 ...");
        bool authSuccess = false;
        yield return StartCoroutine(InitFirebaseRoutine(success => authSuccess = success));

        if (!authSuccess) yield break;
        SetProgress(0.3f);
        
        SetStatus("데이터 불러오는 중...");
        yield return StartCoroutine(LoadDataRoutine());
        SetProgress(0.5f);

        SetStatus("초기화 중...");
        InitManagers();
        SetProgress(1.0f);

        yield return new WaitForSeconds(0.3f);

        progressBar.SetActive(false);
        ShowStartText();
        getAnyKeyCoroutine = StartCoroutine(GetAnyKeyRoutine());
    }

    private IEnumerator InitFirebaseRoutine(Action<bool> onComplete)
    {
        var task = FirebaseAuthManager.Instance.InitAndSignIn();

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogError(task.Exception);
            // 재시도 로직
            // 재시도 UI팝업 재생
            // 확인누르면 씬 재로드
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            onComplete(false);
            yield break;
        }

        bool isSuccess = task.Result;
        if (!isSuccess)
        {
            // 재시도 로직
            // 재시도 UI팝업 재생
            // 확인누르면 씬 재로드
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            onComplete(false);
            yield break;
        }
        
        onComplete(true);
    }

    private IEnumerator LoadDataRoutine()
    {
        string uid = FirebaseAuthManager.Instance.CurrentUser?.UserId;
        
        var storeTask = FirestoreManager.Instance.Init();
        yield return new WaitUntil(() => storeTask.IsCompleted);

        var functionTask = FirebaseFunctionsManager.Instance.Init();
        yield return new WaitUntil(() => functionTask.IsCompleted);

        var saveDataTask = FirestoreManager.Instance.LoadPlayerData(uid);
        yield return new WaitUntil(() => saveDataTask.IsCompleted);

        saveData = saveDataTask.Result;
    }

    private void InitManagers()
    {
        GameManager.Instance.SetSaveData(saveData);
        
        AudioManager.Instance.Init();
        PoolManager.Instance.Init();
        CurrencyManager.Instance.Init(saveData.CurrencySaveData);
        GachaManager.Instance.Init(saveData.PityCount);
    }

    private void SetProgress(float fillAmount)
    {
        progressFill.DOFillAmount(fillAmount, 0.3f);
    }

    private void SetStatus(string msg)
    {
        statusText.text = msg;
    }

    private void ShowStartText()
    {
        startText.gameObject.SetActive(true);
    
        blinkTween = startText.DOFade(0f, 0.8f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    private IEnumerator GetAnyKeyRoutine()
    {
        while (true)
        {
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene("2.MainScene");
                yield break;
            }
            
            yield return null;
        }
    }
}
