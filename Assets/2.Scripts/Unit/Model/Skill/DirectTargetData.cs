using System;
using UnityEngine;

public abstract class DirectTargetData : SkillTargetData
{
    public abstract bool IsInRange(Vector2 casterPos, Vector2 targetPos, Vector2 forward);
}
