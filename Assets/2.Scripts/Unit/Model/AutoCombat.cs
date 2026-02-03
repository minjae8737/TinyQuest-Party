using System;
using UnityEngine;

public class AutoCombat : MonoBehaviour
{
    [SerializeField] private UnitController controller;
    [SerializeField] [Range(0.1f, 0.3f)] private float delay;
    private float lastTime;

    private void Update()
    {
        if (Time.time - lastTime >= delay)
        {
            //스킬 선택
            int nextSkill = DecideNextSkill();

            //공격시도
            if (controller.CanAttack(nextSkill))
            {
                controller.DoAttack(nextSkill, transform.position, Time.time);
            }
            //이동
            else
            {
                if (controller.TryGetNextPos(nextSkill, out Vector2 nextPos))
                {
                    controller.SetNextPos(nextPos);
                }
            }

            lastTime = Time.time;
        }
    }

    private int DecideNextSkill()
    {
        int nextSkill = 0;

        if (controller.CanUseSkill((int)SkillType.Skill3, Time.time))
        {
            nextSkill = (int)SkillType.Skill3;
        }
        else if (controller.CanUseSkill((int)SkillType.Skill2, Time.time))
        {
            nextSkill = (int)SkillType.Skill2;
        }
        else if (controller.CanUseSkill((int)SkillType.Skill1, Time.time))
        {
            nextSkill = (int)SkillType.Skill1;
        }

        return nextSkill;
    }
}