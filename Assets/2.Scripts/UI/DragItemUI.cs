using UnityEngine;
using UnityEngine.UI;

public class DragItemUI : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    [SerializeField] private Image image;

    public void SetSize(Vector2 size)
    {
        rect.sizeDelta = size;
    }

    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }
    
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
