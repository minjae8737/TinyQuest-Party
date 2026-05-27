using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitManagementPanel : UIPage
{
    [Header("=== Unit  ===")] 
    [SerializeField] private TMP_Text UnitNameText;
    [SerializeField] private StarGradeUI StarGradeUI;
    [SerializeField] private TMP_Text LevelText;
    [SerializeField] private Image unitSprite;

    [Header("=== Info ===")] 
    [SerializeField] private Image tapToggleHighlight;
    [SerializeField] private TMP_Text atkStatText;
    [SerializeField] private TMP_Text defStatText;
    [SerializeField] private TMP_Text hpStatText;

    [Header("=== Upgrade ===")] 
    [SerializeField] private Button levelUpgradeBtn;
    [SerializeField] private TMP_Text levelBtnText;
    [SerializeField] private Button starUpgradeBtn;

    [Header("=== UnitCardList ===")] 
    [SerializeField] private RectTransform UnitCardParent;
    [SerializeField] private GridLayoutGroup grid;
    [SerializeField] private GameObject CardUIPrefab;
    
    [SerializeField] private Image classToggleHighlight;
    [SerializeField] private List<Toggle> classToggleGroup;
    
    [SerializeField] private Sprite[] starGradeSprites;
    
    // Cacing
    private Vector2 toggleHighlightOriginSize;
    private List<UnitInfoCardUI> unitCards;

    #region RunTime

    private UnitInfoDTO curUnitInfoDTO;

    #endregion

    public void Init()
    {
        unitCards = new();
        
        // Cacing
        toggleHighlightOriginSize = classToggleHighlight.rectTransform.sizeDelta;
        
        InitUnitListPanel();

        levelUpgradeBtn.onClick.AddListener(() => UIEffect.Punch(levelUpgradeBtn.transform as RectTransform));
        levelUpgradeBtn.onClick.AddListener(OnClickLevelUpButton);
        // starUpgradeBtn.onClick.AddListener(() => UIEffect.Punch(starUpgradeBtn.transform as RectTransform));
        starUpgradeBtn.enabled = false; //TODO 승급시스템 개발중
        UIEffect.PunchLoop(tapToggleHighlight.rectTransform);
        
        // Card - ClassToggle
        foreach (Toggle toggle in classToggleGroup)
        {
            toggle.onValueChanged.AddListener(OnChangedClassToggle);
        }
        classToggleGroup[0].isOn = true;
        UIEffect.PunchLoop(classToggleHighlight.rectTransform);

        UpdateUnitInfo(unitCards.First().UnitName);
    }
    
    public override void Show()
    {
        gameObject.SetActive(true);
        AudioManager.Instance.PlaySfx(Sfx.UIOpen);
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
        AudioManager.Instance.PlaySfx(Sfx.UIClose);
    }

    private void UpdateUnitInfo(UnitName unitName)
    {
        curUnitInfoDTO = UnitManager.Instance.GetUnitInfoDTO(unitName);

        if (curUnitInfoDTO == null)
        {
            Debug.LogError($"Failed UpdateUnitInfo. : {unitName}");
            return;
        }
        
        UnitNameText.text = curUnitInfoDTO.UnitName.ToString();
        StarGradeUI.SetStars(curUnitInfoDTO.StarGrade, starGradeSprites[curUnitInfoDTO.StarGrade]);
        LevelText.text = $"Lv.{curUnitInfoDTO.UnitLevel} / {curUnitInfoDTO.UnitMaxLevel}";
        unitSprite.sprite = curUnitInfoDTO.UnitSprite;

        atkStatText.text = $"{curUnitInfoDTO.Stat.Atk}";
        defStatText.text = $"{curUnitInfoDTO.Stat.Def}";
        hpStatText.text = $"{curUnitInfoDTO.Stat.MaxHp}";


        long maxExp = ExpCalculator.Instance.GetMaxExp(curUnitInfoDTO.UnitLevel);
        string curExpStr = UIManager.Instance.NumberFormatter(CurrencyManager.Instance.Exp);
        string requiredExpStr = UIManager.Instance.NumberFormatter(maxExp);
        
        bool canLevelUp = CurrencyManager.Instance.Exp >= maxExp;
        string color = canLevelUp ? "white" : "red";
        levelUpgradeBtn.enabled = canLevelUp;
        levelBtnText.text = $"<color={color}>{curExpStr}</color> / {requiredExpStr}";
    }

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
            UnitInfoCardUI unitInfoCard = CreateUnitCard(dto);
            unitInfoCard.OnClicked += UpdateUnitInfo;
        }
    }
    
    private UnitInfoCardUI CreateUnitCard(UnitSlotDTO unitSlotDto)
    {
        GameObject unitSlotObj = Instantiate(CardUIPrefab, UnitCardParent);

        if (!unitSlotObj.TryGetComponent<UnitInfoCardUI>(out var card))
        {
            Debug.LogError($"Create card Fail. UnitName : {unitSlotDto.UnitName}");
            return null;
        }

        card.SetSlot(unitSlotDto, starGradeSprites[unitSlotDto.StarGrade]);

        unitCards.Add(card);

        return card;
    }

    private void OnClickLevelUpButton()
    {
        (bool didLevelUp, int level) = curUnitInfoDTO.Unit.LevelUp();

        if (didLevelUp)
        {
            UpdateUnitInfo(curUnitInfoDTO.UnitName);
            UnitInfoCardUI unitInfoCardUI = unitCards.Find(card => { return card.UnitName == curUnitInfoDTO.UnitName; });
            unitInfoCardUI?.SetLevel(level);
        }
    }
    
    private void OnChangedClassToggle(bool isOn)
    {
        int isOnIndex = 0;

        for (int i = 0; i < classToggleGroup.Count; i++)
        {
            if (classToggleGroup[i].isOn)
            {
                classToggleHighlight.rectTransform.parent = classToggleGroup[i].transform;
                classToggleHighlight.rectTransform.anchoredPosition = Vector2.zero;
                classToggleHighlight.rectTransform.sizeDelta = toggleHighlightOriginSize;
                isOnIndex = i;
                break;
            }
        }
        
        // unit 클래스 정렬
        RefreshUnitList(isOnIndex - 1);
    }
    
    private void RefreshUnitList(int selectedClass)
    {
        foreach (var card in unitCards)
        {
            bool isMatch = card.UnitClass == (UnitClass)selectedClass 
                           || !Enum.IsDefined(typeof(UnitClass),selectedClass); // -1 == AllClass
            
            card.gameObject.SetActive(isMatch);

        }
    }
}
