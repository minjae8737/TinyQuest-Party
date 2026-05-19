using System.Collections.Generic;
using UnityEngine;

public enum WaveType
{
    Normal,
    Rush,
    Elite,
    Boss
}

[CreateAssetMenu(menuName = "Stage/WavePattern")]
public class WavePattern : ScriptableObject
{
    public WaveType WaveType;
    public List<EnemyWave> waves;
}
