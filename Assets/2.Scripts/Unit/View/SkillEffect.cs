using System;
using UnityEngine;

public class SkillEffect : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private RuntimeAnimatorController baseController;
    private AnimatorOverrideController aoc;
    
    private const string stateName = "Effect";
    private const string dummyClipName = "Attack_Normal_Effect";

    private Vector2? targetDir;
    

    private void Awake()
    {
        aoc = new AnimatorOverrideController(baseController);
    }

    public void Init()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
        targetDir = null;
    }

    private void Update()
    {
        if (targetDir.HasValue)
        {
            DoMove();
        }
    }

    public void Rotate(Vector2 dir)
    {
    }

    private void DoMove()
    {
        Vector3 myPos = transform.position;
        // Vector2.MoveTowards(myPos, myPos + (targetDir*), Time.deltaTime);
    }

    public void Play(AnimationClip clip)
    {
        aoc[dummyClipName] = clip;
        animator.runtimeAnimatorController = aoc;
        animator.Play(stateName, 0, 0f);
    }

    public void OnEffectOff()
    {
        UnitManager.Instance.DespawnSkillEffect(this);
    }
}