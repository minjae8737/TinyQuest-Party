using System;
using UnityEngine;

public class SkillEffect : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private RuntimeAnimatorController baseController;
    private AnimatorOverrideController aoc;
    
    private const string stateName = "Effect";
    private const string dummyClipName = "Attack_Normal_Effect";

    private void Awake()
    {
        aoc = new AnimatorOverrideController(baseController);
    }

    public void Play(AnimationClip clip, float duration)
    {
        aoc[dummyClipName] = clip;
        animator.runtimeAnimatorController = aoc;

        // 도착시간 duration 에 맞춰서 animator speed 조절
        animator.speed = duration > 0f ? clip.length / duration : 1f;
        
        animator.Play(stateName, 0, 0f);
    }

    public void OnEffectOff()
    {
        UnitManager.Instance.DespawnSkillEffect(this);
    }
}