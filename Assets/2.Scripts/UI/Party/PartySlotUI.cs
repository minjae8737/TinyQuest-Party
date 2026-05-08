using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartySlotUI : DragSlotUI, IDropHandler
{
    [Header("=== Reference ===")]
    [SerializeField] private Image unitImage;
    [SerializeField] private TMP_Text unitNameText;
    [SerializeField] private TMP_Text unitLevelText;
    [SerializeField] private RectTransform starParent;

    [SerializeField] private GameObject Blank;

    [Header("=== Resource ===")]
    [SerializeField] private GameObject starPrefab;

    private PartyUnitDTO data;
    private int slotIdx;
    private List<GameObject> stars = new();
    
    public void SetSlot(PartyUnitDTO partyUnitDto, Sprite starSprite, int idx)
    {
        data = partyUnitDto;
        bool hasUnit = data.UnitName != UnitName.None;

        Blank.SetActive(!hasUnit);
        
        if (!hasUnit) return;
        
        unitImage.sprite = data.Data.Icon;
        unitNameText.text = data.Data.UnitName+"";
        unitLevelText.text = $"Lv.{data.unitLevel}";
        slotIdx = idx;
        
        // 별 세팅
        int starsCount = stars.Count;
        for (int i = 0; i < data.starGrade - starsCount; i++)
        {
            GameObject star = Instantiate(starPrefab, starParent);
            stars.Add(star);
        }
        
        for (int i = 0; i < stars.Count; i++)
        {
            if (i < data.starGrade)
            {
                Image starImg = stars[i].GetComponent<Image>();
                starImg.sprite = starSprite;
                stars[i].SetActive(true);
            }
            else
            {
                stars[i].SetActive(false);
            }
        }
    }

    #region DragEvent

    protected override Image GetDragImage()
    {
        return unitImage;
    }

    protected override bool CanDrag()
    {
        return data.Data.UnitName != UnitName.None;
    }
    
    #endregion

    public override void SetDragContext()
    {
        UnitDragContext dragContext = new UnitDragContext();
        dragContext.source = this;
        dragContext.UnitName = data.Data.UnitName;
        
        UIManager.Instance.DragContext = dragContext;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");

        UnitDragContext dragContext = (UnitDragContext)UIManager.Instance.DragContext;

        if (dragContext.source is PartySlotUI || dragContext.source is UnitListSlotUI)
        {
            UnitManager.Instance.AssignUnitToSlot(slotIdx, dragContext.UnitName);
        }
    }
}
