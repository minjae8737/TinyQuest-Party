using System.Collections.Generic;
using UnityEngine;

public class SkillScanResult
{
    public List<UnitController> Targets = new();
    public List<Vector2> SpawnPos = new();
    public List<Vector2> TargetPos = new();
    
    public UnitController PrimaryTarget;
}
