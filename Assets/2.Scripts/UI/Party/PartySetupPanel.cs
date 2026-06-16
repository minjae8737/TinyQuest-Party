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
    [SerializeField] private GridLayoutGroup grid;
    
    [SerializeField] private Image toggleHighlight;
    [SerializeField] private List<Toggle> classToggleGroup;
    
    [Header("=== Prefabs ===")]
    [SerializeField] private GameObject partySlotPrefab;
    [SerializeField] private GameObject unitSlotPrefab;

    [Header("=== Resources ===")] 
    [SerializeField] private Sprite emptyPartySlotSprite;
    [SerializeField] private Sprite[] starGradeSprites;

    [Header("=== Caching ===")]
    private Vector2 panelGroupOriginPos;
    private Vector2 toggleHighlightOriginSize;
    
    private List<PartySlotUI> partySlotUis;
    private List<PartyCardUI> partyCardUis;

    private PartySlotUI curPartySlot;
    private PartyCardUI curPartyCard;
    
    private int currentPageIdx;
    public static bool IsSetUpMode = false;

    public void Init()
    {
        // PartyPanel
        partySlotUis = new();
        
        for (int i = 0; i < UnitManager.MaxPartySize; i++)
        {
            CreatePartySlot();
        }
        
        RefreshPartyPanel();
        
        // UnitListPanel
        partyCardUis = new();
        InitUnitListPanel();
        
        classToggleGroup[0].isOn = true;
        foreach (Toggle toggle in classToggleGroup)
        {
            toggle.onValueChanged.AddListener(OnChangedToggle);
            toggle.onValueChanged.AddListener(_ => AudioManager.Instance.PlaySfx(Sfx.ChangeToggle));
        }
        UIEffect.PunchLoop(toggleHighlight.rectTransform);

        // Caching
        panelGroupOriginPos = panelGroup.anchoredPosition;
        toggleHighlightOriginSize = toggleHighlight.rectTransform.sizeDelta;
        
        UnitManager.Instance.OnPartyChanged += RefreshPartyPanel;
        
    }
    
    public override void Show()
    {
        RefreshPartyPanel();
        
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

        partySlot.partySetupPanel = this;
        partySlotUis.Add(partySlot);
    }
    
    public void RefreshPartyPanel()
    {
        List<UnitSlotDTO> unitSlotDtos = UnitManager.Instance.GetPartyData();
        
        for (int i = 0; i < unitSlotDtos.Count; i++)
        {
            UnitSlotDTO unitSlotDto = unitSlotDtos[i];
            
            partySlotUis[i].SetSlot(unitSlotDto, starGradeSprites[(int)unitSlotDto.UnitGradeType], i);
        }
    }
    
    public void SelectPartySlot(PartySlotUI partySlot)
    {
        if (IsSetUpMode)
        {
            partySlot.ReplaceUnit(curPartyCard.UnitName);
            ExitPartySetupMode();
        }
        else
        {
            if (curPartySlot == partySlot)
            {
                curPartySlot.UnSelect();
                curPartySlot = null;
                return;
            }
            
            curPartySlot?.UnSelect();
            curPartySlot = partySlot;
            curPartySlot.Select();
        }
    }

    #endregion

    #region UnitListPanel

    private void InitUnitListPanel()
    {
        float cellSizeX = grid.cellSize.x;
        float spacingX = grid.spacing.x;
        int constrainCount = grid.constraintCount;
        float width = (grid.transform as RectTransform).rect.width;

        float sizeX = width - (cellSizeX * constrainCount) - (spacingX * constrainCount - 1);
        grid.padding.left = grid.padding.right = (int)sizeX / 2;

        List<UnitSlotDTO> unitSlotDtos = UnitManager.Instance.GetPlayerUnitSlotDTO();

        foreach (var dto in unitSlotDtos)
        {
            CreateUnitSlot(dto);
        }
    }

    private PartyCardUI CreateUnitSlot(UnitSlotDTO unitSlotDto)
    {
        GameObject unitSlotObj = Instantiate(unitSlotPrefab, unitSlotScrollRect);

        if (!unitSlotObj.transform.GetChild(0).TryGetComponent<PartyCardUI>(out var unitSlot))
        {
            Debug.LogError($"Create UnitListItemUI Fail. Unitname : {unitSlotDto.UnitName}");
            return null;
        }

        unitSlot.partySetupPanel = this;
        unitSlot.SetSlot(unitSlotDto, starGradeSprites[(int)unitSlotDto.UnitGradeType]);

        partyCardUis.Add(unitSlot);

        return unitSlot;
    }

    public void SetPartyCard(UnitName unitName, int grade)
    {
        var partyCard = partyCardUis.Find(c => c.UnitName == unitName);
        
        partyCard?.SetStars(grade);
    }
    
    private void SortUnitListPanel(int selectedClass)
    {
        foreach (var partyCard in partyCardUis)
        {
            bool isMatch = partyCard.UnitClass == (UnitClass)selectedClass 
                           || !Enum.IsDefined(typeof(UnitClass),selectedClass); // -1 == AllClass
            
            partyCard.transform.parent.gameObject.SetActive(isMatch);
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
        SortUnitListPanel(isOnIndex - 1);
    }

    public void SelectUnitSlot(PartyCardUI partyCard)
    {
        if (curPartyCard == partyCard)
        {
            curPartyCard.UnSelect();
            curPartyCard = null;
            return;
        }
        
        curPartyCard?.UnSelect();
        UIEffect.StopShakeLoop(curPartyCard?.transform as RectTransform);
        
        curPartyCard = partyCard;
        curPartyCard.Select();
    }

    public void EnterPartySetupMode()
    {
        curPartySlot?.UnSelect();
        curPartyCard?.UnSelect();
        curPartySlot = null;
        UIEffect.StartShakeLoop(curPartyCard?.transform as RectTransform);
        IsSetUpMode = true;
    }
    
    public void ExitPartySetupMode()
    {
        UIEffect.StopShakeLoop(curPartyCard?.transform as RectTransform);
        IsSetUpMode = false;
    }
    
    #endregion
}
