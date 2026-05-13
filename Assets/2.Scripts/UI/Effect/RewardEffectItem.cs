using UnityEngine;
using UnityEngine.UI;

public class RewardEffectItem : Poolable
{
    [SerializeField] private Image image;
    
    public void Init(Sprite sprite)
    {
        image.sprite = sprite;
        transform.localScale = Vector3.one;
    }
}
