using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Functions;
using UnityEngine;

public class GachaResult
{
    public string UnitName;
    public string Grade;
}

public class GachaManager : Singleton<GachaManager>
{
    FirebaseFunctions functions;

    public async void Init()
    {
        functions = FirebaseFunctions.GetInstance("us-central1");
        Debug.Log("GachaManager Init Complete.");
    }

    public async Task<GachaResult> DoGacha()
    {
        try
        {
            HttpsCallableReference callable = functions.GetHttpsCallable("gacha");
            HttpsCallableResult result = await callable.CallAsync();

            // 결과 파싱
            var data = result.Data as Dictionary<string, object>;
            return new GachaResult
            {
                UnitName = data["unitName"].ToString(),
                Grade    = data["grade"].ToString(),
            };
        }
        catch (FunctionsException e)
        {
            Debug.LogError($"뽑기 실패: {e.ErrorCode} - {e.Message}");
            return null;
        }
        catch (Exception e)
        {
            Debug.LogError($"Exception: {e.GetType().Name} - {e.Message}");
            return null;
        }
    }

    private async void TestGacha()
    {
        GachaResult result = await DoGacha();
        if (result != null)
            Debug.Log($"결과: {result.Grade} {result.UnitName}");
    }
    
    private async void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TestGacha();
        }
    }
}
