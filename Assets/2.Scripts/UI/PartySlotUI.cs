using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartySlotUI : MonoBehaviour, DraggedItem,IDropHandler
{
    [Header("=== Reference ===")]
    [SerializeField] private Image unitImage;
    [SerializeField] private Text unitNameText;

    private UnitName unitName;
    private int slotIdx;
    
    public DragItemUI dragItemUI;

    public void SetSlot(Sprite unitSptrite, UnitName unitName, int idx)
    {
        unitImage.sprite = unitSptrite;
        unitNameText.text = unitName+"";
        this.unitName = unitName;
        slotIdx = idx;
    }

    #region DragEvent

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        if (unitName == UnitName.None) return;
        
        dragItemUI = UIManager.Instance.GetDragItem();
        dragItemUI.SetActive(true);
        dragItemUI.SetSize(unitImage.rectTransform.sizeDelta);
        dragItemUI.SetSprite(unitImage.sprite);

        dragItemUI.transform.position = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        if (unitName == UnitName.None) return;
        
        dragItemUI.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        dragItemUI.SetActive(false);
        dragItemUI = null;
    }
    
    #endregion

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        
    }
}
