using Unity.VisualScripting;
using UnityEngine;

public enum AnimName
{
    Idle,
    Walk,
    Hurt,
    Death,
    Attack_Normal = 50,
    Attack_Skill1,
    Attack_Skill2,
    Attack_Skill3,
}

public class UnitView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Animator anim;

    public void SetSpeed(float speed)
    {
        anim.SetFloat(AnimName.Walk.ToString(), speed); // Idle <-> Walk
    }
    
    public void SetFlipX(bool isLeft)
    {
        if (isLeft == sr.flipX) return;
        sr.flipX = isLeft;
    }

    public void SetOrderInLayer(int order)
    {
        if (sr.sortingOrder == order) return;
        sr.sortingOrder = order;
    }

    public void PlayAttack(int skillIdx)
    {
        switch (skillIdx)
        { 
            case 0:
                anim.SetTrigger(AnimName.Attack_Normal.ToString());
                break;
            case 1:
                anim.SetTrigger(AnimName.Attack_Skill1.ToString());
                break;
            case 2:
                anim.SetTrigger(AnimName.Attack_Skill2.ToString());
                break;
        }
    }

    public void PlayDeath()
    {
        anim.SetTrigger(AnimName.Death.ToString());
    }
}