using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Functions;
using UnityEngine;

public class FirebaseFunctionsManager : Singleton<FirebaseFunctionsManager>
{
    private FirebaseFunctions functions;
    
    public async Task Init()
    {
        functions = FirebaseFunctions.GetInstance("us-central1");
    }
    
    public async Task<GachaResponse> RequestGacha(int count)
    {
        try
        {
            HttpsCallableReference callable = functions.GetHttpsCallable("gacha");
            var requestData = new Dictionary<string, object> { { "count", count } };

            var result = await callable.CallAsync(requestData);
            var raw = result.Data as Dictionary<string, object>;

            var rawResults = raw["results"] as List<object>;
            var gachaResults = rawResults
                .Cast<Dictionary<string, object>>()
                .Select(r => new GachaResult
                {
                    UnitName = r["unitName"] as string,
                    Grade = Enum.Parse<UnitGradeType>(r["grade"] as string)
                })
                .ToList();

            return new GachaResponse
            {
                Results = gachaResults,
                TotalCost = Convert.ToInt32(raw["totalCost"]),
                PityCount = Convert.ToInt32(raw["pityCount"])
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
}
