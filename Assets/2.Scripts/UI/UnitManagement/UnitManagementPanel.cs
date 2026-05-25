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
    [SerializeField] private TMP_Text atkStatText;
    [SerializeField] private TMP_Text defStatText;
    [SerializeField] private TMP_Text hpStatText;

    [Header("=== Upgrade ===")] 
    [SerializeField] private Button levelUpgradeBtn;
    [SerializeField] private TMP_Text levelBtnText;
    [SerializeField] private Button starUpgradeBtn;

    [Header("===  ===")] 
    [SerializeField] private RectTransform UnitCardParent;
    [SerializeField] private GameObject CardUIPrefab;
    [SerializeField] private Sprite[] starGradeSprites;

    private List<CardUI> unitCards;

    #region RunTime

    private UnitInfoDTO curUnitInfoDTO;

    #endregion

    public void Init()
    {
        unitCards = new();
        
        InitUnitListPanel();

        levelUpgradeBtn.onClick.AddListener(() => UIEffect.Punch(levelUpgradeBtn.transform as RectTransform));
        levelUpgradeBtn.onClick.AddListener(OnClickLevelUpButton);
        // starUpgradeBtn.onClick.AddListener(() => UIEffect.Punch(starUpgradeBtn.transform as RectTransform));
        starUpgradeBtn.enabled = false; //TODO 승급시스템 개발중

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
        List<UnitSlotDTO> unitSlotDtos = UnitManager.Instance.GetPlayerUnitSlotDTO();

        foreach (var dto in unitSlotDtos)
        {
            CardUI card = CreateUnitCard(dto);
            card.OnClicked += UpdateUnitInfo;
        }
    }
    
    private CardUI CreateUnitCard(UnitSlotDTO unitSlotDto)
    {
        GameObject unitSlotObj = Instantiate(CardUIPrefab, UnitCardParent);

        if (!unitSlotObj.TryGetComponent<CardUI>(out var card))
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
            CardUI cardUI = unitCards.Find(card => { return card.UnitName == curUnitInfoDTO.UnitName; });
            cardUI?.SetLevel(level);
        }
    }
}
