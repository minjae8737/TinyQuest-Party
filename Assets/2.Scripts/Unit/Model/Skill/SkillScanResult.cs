using System.Collections.Generic;
using UnityEngine;

public class SkillScanResult
{
    public List<UnitController> Targets = new();
    public List<Vector2> EffectPos = new();
    public UnitController PrimaryTarget;
}
