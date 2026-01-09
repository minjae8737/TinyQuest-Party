using Unity.VisualScripting;
using UnityEngine;

public enum AnimName
{
    Idle,
    Speed,
    Hurt,
    Death,
    Attack1 = 50,
    Attack2,
    Attack3
}

public class CharacterView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Animator anim;

    public void SetSpeed(float speed)
    {
        anim.SetFloat(AnimName.Speed.ToString(), speed);
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

    public void AttackAnim(int skillIdx)
    {
        switch (skillIdx)
        {
            case 0:
                anim.SetTrigger(AnimName.Attack1.ToString());
                break;
            case 1:
                anim.SetTrigger(AnimName.Attack2.ToString());
                break;
            case 2:
                anim.SetTrigger(AnimName.Attack3.ToString());
                break;
        }
    }
}