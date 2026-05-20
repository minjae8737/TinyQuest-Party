using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage_00000",menuName = "Stage/Data")]
public class StageData : ScriptableObject
{
    public List<IslandData> IslandDatas; // 삭제 예정

    [Header("=== INFO ===")]
    public int startStage;
    public int endStage;

    [Range(3, 4)] public int islandCount;
    [Range(1, 3)] public int WaveCount;
    
    [Space(10)]
    public List<WavePattern> Patterns;
    public RewardData RewardData;

    public bool ContainsLevel(int curStageLevel)
    {
        return startStage <= curStageLevel && curStageLevel <= endStage;
    }

    public WavePattern PickUpPattern(WaveType type)
    {
        List<WavePattern> patterns = Patterns.FindAll(p => p.WaveType == type);

        if (patterns.Count == 0)
        {
            Debug.LogError($"No Pattern : {type}");
            return null;
        }

        int randIdx = Random.Range(0, patterns.Count);
        return patterns[randIdx];
    }

    public WaveType DecideWaveType(int islandIndex, int waveIndex)
    {
        //TODO Boss Stage 추후 추가 예정 (Stage 마지막 Wave)

        if (waveIndex % 4 == 3) // 3번째 웨이브마다 엘리트
            return WaveType.Elite;

        if (Random.value < 0.25f) // 랜덤으로 러시
            return WaveType.Rush;

        return WaveType.Normal;
    }
}
