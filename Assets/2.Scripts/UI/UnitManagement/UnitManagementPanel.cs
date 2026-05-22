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

    public void Init()
    {
        unitCards = new();
        
        InitUnitListPanel();

        levelUpgradeBtn.onClick.AddListener(() => UIEffect.Punch(levelUpgradeBtn.transform as RectTransform));
        // starUpgradeBtn.onClick.AddListener(() => UIEffect.Punch(starUpgradeBtn.transform as RectTransform));
        starUpgradeBtn.enabled = false; //TODO 승급시스템 개발중

        InitUnitListPanel();
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
        UnitInfoDTO dto = UnitManager.Instance.GetUnitInfoDTO(unitName);

        if (dto == null)
        {
            Debug.LogError($"Failed UpdateUnitInfo. : {unitName}");
            return;
        }
        
        UnitNameText.text = dto.UnitName.ToString();
        StarGradeUI.SetStars(dto.StarGrade, starGradeSprites[dto.StarGrade]);
        LevelText.text = $"Lv.{dto.UnitLevel} / {dto.UnitMaxLevel}";
        unitSprite.sprite = dto.Data.Icon;

        atkStatText.text = $"{dto.Stat.Atk}";
        defStatText.text = $"{dto.Stat.Def}";
        hpStatText.text = $"{dto.Stat.MaxHp}";


        long maxExp = ExpCalculator.Instance.GetMaxExp(dto.UnitLevel);
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
            CreateUnitCard(dto);
        }
    }
    
    private CardUI CreateUnitCard(UnitSlotDTO unitSlotDto)
    {
        GameObject unitSlotObj = Instantiate(CardUIPrefab, UnitCardParent);

        if (!unitSlotObj.TryGetComponent<CardUI>(out var card))
        {
            Debug.LogError($"Create card Fail. Unitname : {unitSlotDto.UnitName}");
            return null;
        }

        card.SetSlot(unitSlotDto, starGradeSprites[unitSlotDto.StarGrade]);

        unitCards.Add(card);

        return card;
    }
}
