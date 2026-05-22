using UnityEngine;
using UnityEngine.EventSystems;

public class ClickSlotUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private float dragThreshold = 15f;

    private Vector2 downPos;
    protected bool isScrolling;

    public void OnPointerDown(PointerEventData eventData)
    {
        downPos = eventData.position;
    }
    

    public void OnPointerUp(PointerEventData eventData)
    {
        float distance = Vector2.Distance(downPos, eventData.position);

        isScrolling = distance > dragThreshold;
        
        if (!isScrolling)
        {
            OnClickCard();
        }
    }

    protected virtual void OnClickCard()
    {
    }
}