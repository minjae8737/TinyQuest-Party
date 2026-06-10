using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GachaResult
{
    public string UnitName;
    public UnitGradeType Grade;
}

public class GachaResponse
{
    public List<GachaResult> Results;
    public int TotalCost;
    public int PityCount;
}

public class GachaManager : Singleton<GachaManager>
{
    private int pityCount; // 천장 스택

    public int PityCount => pityCount;


    public async Task<GachaResponse> DoGacha(int count = 1)
    {
        var gachaResult = await FirebaseFunctionsManager.Instance.RequestGacha(count);
        
        pityCount = gachaResult.PityCount;
        
        return gachaResult;
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
