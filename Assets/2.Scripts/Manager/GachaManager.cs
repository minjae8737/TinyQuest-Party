using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Functions;
using UnityEngine;

public class GachaResult
{
    public UnitName UnitName;
    public UnitGradeType Grade;
}

public class GachaResponse
{
    public List<GachaResult> Results;
    public int TotalCost;
    public int PityCount;
}

public class GachaResultData
{
    public UnitName UnitName;
    public Sprite Icon;
    public UnitGradeType UnitGradeType;
}

public class GachaManager : Singleton<GachaManager>
{
    private int pityCount; // 천장 스택

    public int PityCount => pityCount;

    public event Action<int> OnChangedPityCount;

    public async Task<List<GachaResultData>> DoGacha(int count = 1)
    {
        try
        {
            var gachaResult = await FirebaseFunctionsManager.Instance.RequestGacha(count);
            
            // 천장 스택 변경
            pityCount = gachaResult.PityCount;
            OnChangedPityCount?.Invoke(pityCount);
            
            // 재화 소모
            CurrencyManager.Instance.SpendGold(gachaResult.TotalCost);
            
            // 패널용 데이터 가공
            List<GachaResultData> results = new();

            foreach (var result in gachaResult.Results)
            {
                GachaResultData resultData = new();
                PlayerUnitData unitData = UnitManager.Instance.GetPlayerUnitData(result.UnitName);
                if (unitData != null)
                {
                    resultData.UnitName = result.UnitName;
                    resultData.Icon = unitData.Icon;
                    resultData.UnitGradeType = unitData.UnitGradeType;
                    results.Add(resultData);
                }
                else
                {
                    Debug.LogError($"Gacha: {result.UnitName} UnitData is null.");
                }


                // 프래그먼츠 처리
                UnitManager.Instance.AddUnitFragment(result.UnitName);
            }
            
            return results;
        }
        catch (FunctionsException e)
        {
            HandleFunctionsException(e);
            return null;
        }
    }

    private void HandleFunctionsException(FunctionsException e)
    {
        switch (e.ErrorCode)
        {
            case FunctionsErrorCode.Unauthenticated:    // 로그인이 필요합니다
                break;
            
            case FunctionsErrorCode.InvalidArgument:    // 유효하지 않은 뽑기 수입니다
                break;
            
            case FunctionsErrorCode.NotFound:           // 유저 없음
                break;
            
            case FunctionsErrorCode.FailedPrecondition: // 골드가 부족합니다
                break;
            
            default:
                Debug.LogError($"알 수 없는 오류: {e.ErrorCode} - {e.Message}");
                break;
        }
    }

    // private async void TestGacha()
    // {
    //     GachaResponse result = await DoGacha(10);
    //     StringBuilder sb = new();
    //
    //     sb.AppendLine($"PityCount : {result.PityCount}");
    //
    //     foreach (var gachaResult in result.Results)
    //     {
    //         sb.Append($"UnitName : {gachaResult.UnitName} / {gachaResult.Grade} \n");
    //     }
    //
    //
    //     Debug.Log(sb);
    // }
    
    // private async void LateUpdate()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         TestGacha();
    //     }
    // }
}
