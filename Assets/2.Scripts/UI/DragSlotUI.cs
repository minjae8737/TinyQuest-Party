using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class DragSlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] protected DragItemUI dragItemUI;
    protected abstract Image GetDragImage();
    protected abstract bool CanDrag();
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Debug.Log("OnBeginDrag");
        if (!CanDrag()) return;
        
        dragItemUI = UIManager.Instance.GetDragItem();
        dragItemUI.SetActive(true);
        dragItemUI.SetSize(GetDragImage().rectTransform.sizeDelta);
        dragItemUI.SetSprite(GetDragImage().sprite);
        SetDragContext();
        
        dragItemUI.transform.position = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("OnDrag");
        if (!CanDrag()) return;
        
        dragItemUI.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Debug.Log("OnEndDrag");
        if (!CanDrag()) return;

        dragItemUI.SetActive(false);
        dragItemUI = null;
    }

    public abstract void SetDragContext();
}
