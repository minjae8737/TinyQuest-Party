using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartySetupPanel : UIPage
{
    [Header("=== References ===")]
    [SerializeField] private RectTransform panelGroup;
    [SerializeField] private RectTransform partySlotParent;
    [SerializeField] private RectTransform unitSlotScrollRect;
    
    [SerializeField] private Image toggleHighlight;
    [SerializeField] private List<Toggle> classToggleGroup;
    
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    
    [Header("=== Prefabs ===")]
    [SerializeField] private GameObject partySlotPrefab;
    [SerializeField] private GameObject pagePrefab;
    [SerializeField] private GameObject unitSlotPrefab;

    [Header("=== Resources ===")] 
    [SerializeField] private Sprite emptyPartySlotSprite;
    [SerializeField] private Sprite[] starGradeSprites;

    [Header("=== Caching ===")]
    private Vector2 panelGroupOriginPos;
    private Vector2 toggleHighlightOriginSize;
    
    private List<PartySlotUI> partySlotUis;
    private List<UnitSlotPage> unitSlotPages;
    private List<UnitSlotUI> unitSlotUis;
    
    private UnitSlotPage currentPage;
    private int currentPageIdx;
    private float PageWidth = 990f;

    public void Init()
    {
        Debug.Log("PartySetupPanel Init");
        // PartyPanel
        partySlotUis = new();
        
        for (int i = 0; i < UnitManager.MaxPartySize; i++)
        {
            CreatePartySlot();
        }
        
        RefreshPartyPanel();
        
        // UnitListPanel
        unitSlotPages = new();
        unitSlotUis = new();
        InitUnitListPanel();
        
        foreach (Toggle toggle in classToggleGroup)
        {
            toggle.onValueChanged.AddListener(OnChangedToggle);
        }
        classToggleGroup[0].isOn = true;
        UIEffect.PunchLoop(toggleHighlight.rectTransform);
        
        leftButton.onClick.RemoveAllListeners();
        rightButton.onClick.RemoveAllListeners();
        leftButton.onClick.AddListener(() => UIEffect.Punch(leftButton.transform as RectTransform));
        leftButton.onClick.AddListener(() => OnClickPageButton(false));
        rightButton.onClick.AddListener(() => UIEffect.Punch(rightButton.transform as RectTransform));
        rightButton.onClick.AddListener(() => OnClickPageButton(true));
        
        // Caching
        panelGroupOriginPos = panelGroup.anchoredPosition;
        toggleHighlightOriginSize = toggleHighlight.rectTransform.sizeDelta;
        
        UnitManager.Instance.OnPartyChanged += RefreshPartyPanel;
        // UnitManager.Instance.OnPartyChanged += RefreshUnitListPanel;
    }
    
    public override void Show()
    {
        gameObject.SetActive(true);
        AudioManager.Instance.PlaySfx(Sfx.UIOpen);
        UIEffect.SlideUp(panelGroup, panelGroupOriginPos, 50f, 0.7f);
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
        AudioManager.Instance.PlaySfx(Sfx.UIClose);
    }

    #region PartyPanel

    private void CreatePartySlot()
    {
        GameObject partySlotObj = Instantiate(partySlotPrefab, partySlotParent);
        
        if (!partySlotObj.TryGetComponent<PartySlotUI>(out var partySlot))
        {
            Debug.LogError("CreatePartySlotUI Fail");
            return;
        } 
        
        partySlotUis.Add(partySlot);
    }
    
    public void RefreshPartyPanel()
    {
        List<UnitSlotDTO> unitSlotDtos = UnitManager.Instance.GetPartyData();
        
        for (int i = 0; i < unitSlotDtos.Count; i++)
        {
            UnitSlotDTO unitSlotDto = unitSlotDtos[i];
            
            partySlotUis[i].SetSlot(unitSlotDto, starGradeSprites[unitSlotDto.StarGrade], i);
        }
    }

    #endregion

    #region UnitListPanel

    private void InitUnitListPanel()
    {
        currentPage = null;

        List<UnitSlotDTO> unitSlotDtos = UnitManager.Instance.GetPlayerUnitSlotDTO();

        foreach (var dto in unitSlotDtos)
        {
            CreateUnitSlot(dto);
        }
    }
    
    private UnitSlotPage CreatePage()
    {
        GameObject pageObj = Instantiate(pagePrefab, unitSlotScrollRect);

        if (!pageObj.TryGetComponent<UnitSlotPage>(out var page))
        {
            Debug.LogError($"Create UnitSlotPage Fail.");
            return null;
        }
        
        unitSlotPages.Add(page);
        return page;
    }

    private UnitSlotUI CreateUnitSlot(UnitSlotDTO unitSlotDto)
    {
        if (currentPage == null || currentPage.IsFull)
        {
            currentPage = CreatePage();
        }
        
        GameObject unitSlotObj = Instantiate(unitSlotPrefab, currentPage.transform);

        if (!unitSlotObj.TryGetComponent<UnitSlotUI>(out var unitSlot))
        {
            Debug.LogError($"Create UnitListItemUI Fail. Unitname : {unitSlotDto.UnitName}");
            return null;
        }

        unitSlot.SetSlot(unitSlotDto, starGradeSprites[unitSlotDto.StarGrade]);

        currentPage.Add(unitSlot);
        unitSlotUis.Add(unitSlot);

        return unitSlot;
    }

    private void RefreshUnitListPanel(int selectedClass)
    {
        foreach (var page in unitSlotPages)
        {
            page.Clear();
        }
        
        List<UnitSlotUI> filteredSlot = new();
        
        foreach (var unitSlot in unitSlotUis)
        {
            bool isMatch = unitSlot.UnitClass == (UnitClass)selectedClass 
                           || !Enum.IsDefined(typeof(UnitClass),selectedClass); // -1 == AllClass
            
            unitSlot.gameObject.SetActive(isMatch);

            if (isMatch) filteredSlot.Add(unitSlot);
        }
        
        // UnitName 알파벳 순 정렬
        filteredSlot.Sort((a, b) => string.Compare(a.UnitName.ToString(), b.UnitName.ToString()));

        int pageIdx = 0;
        
        foreach (UnitSlotUI unitSlot in filteredSlot)
        {
            UnitSlotPage page = unitSlotPages[pageIdx];

            if (page.IsFull)
            {
                page = unitSlotPages[++pageIdx];
            }
            
            page.Add(unitSlot);
        }
    }

    private void OnChangedToggle(bool isOn)
    {
        int isOnIndex = 0;

        for (int i = 0; i < classToggleGroup.Count; i++)
        {
            if (classToggleGroup[i].isOn)
            {
                toggleHighlight.rectTransform.parent = classToggleGroup[i].transform;
                toggleHighlight.rectTransform.anchoredPosition = Vector2.zero;
                toggleHighlight.rectTransform.sizeDelta = toggleHighlightOriginSize;
                isOnIndex = i;
                break;
            }
        }
        
        // unit 클래스 정렬
        RefreshUnitListPanel(isOnIndex - 1);
    }

    private void OnClickPageButton(bool isNext)
    {
        int nextPageIdx = currentPageIdx + (isNext ? 1 : -1);
        nextPageIdx = Mathf.Clamp(nextPageIdx, 0, unitSlotPages.Count - 1);
        
        if (unitSlotPages[nextPageIdx].IsEmpty) return;
        currentPageIdx = nextPageIdx;  
        
        MovePage(currentPageIdx);
    }

    private void MovePage(int pageIdx)
    {
        Vector2 position = unitSlotScrollRect.anchoredPosition;
        float width = unitSlotScrollRect.sizeDelta.x;
        position.x = width * -pageIdx;

        UIEffect.Scrolling(unitSlotScrollRect, position);
    }
    
    #endregion
}
