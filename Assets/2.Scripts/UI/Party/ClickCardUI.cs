using UnityEngine;
using UnityEngine.EventSystems;

public class ClickCardUI : MonoBehaviour, IPointerClickHandler
{
    private int dragThreshold = 30;

    void Start()
    {
        EventSystem.current.pixelDragThreshold = dragThreshold;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickCard();
    }

    protected virtual void OnClickCard()
    {
    }
}