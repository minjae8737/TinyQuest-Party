using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitListSlotUI : DragSlotUI
{
    [Header("=== Reference ===")]
    [SerializeField] private Image unitImage;
    [SerializeField] private TMP_Text unitNameText;
    [SerializeField] private TMP_Text unitLevelText;
    [SerializeField] private RectTransform starParent;
    
    [Header("=== Resource ===")]
    [SerializeField] private GameObject starPrefab;
    
    private Unit unit;
    private List<GameObject> stars = new();
    

    public void SetSlot(Unit unit, Sprite starSprite)
    {
        this.unit = unit;
        
        unitImage.sprite = unit.Data.Icon;
        unitNameText.text = unit.Data.UnitName+"";
        unitLevelText.text = $"Lv.{unit.Level.Level}";
        
        // 별 세팅
        int starsCount = stars.Count;
        for (int i = 0; i < unit.StarGrade - starsCount; i++)
        {
            GameObject star = Instantiate(starPrefab, starParent);
            stars.Add(star);
        }

        for (int i = 0; i < stars.Count; i++)
        {
            Image starImg = stars[i].GetComponent<Image>();
            starImg.sprite = starSprite;
            stars[i].SetActive(i < unit.StarGrade);
        }
    }
    
    #region DragEvent
    
    protected override Image GetDragImage()
    {
        return unitImage;
    }

    protected override bool CanDrag()
    {
        return unit.Data.UnitName != UnitName.None;
    }
    
    #endregion
    
    public override void SetDragContext()
    {
        UnitDragContext dragContext = new UnitDragContext();
        dragContext.source = this;
        dragContext.UnitName = unit.Data.UnitName;
        
        UIManager.Instance.DragContext = dragContext;
    }
    
}
