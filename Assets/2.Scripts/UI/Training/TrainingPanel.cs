using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrainingPanel : MonoBehaviour
{
    [Header("=== Training Level Group ===")] 
    [SerializeField] private Button minusBtn;
    [SerializeField] private TextMeshProUGUI trainingLevelText;
    [SerializeField] private Button plusBtn; 
    
    [Header("=== Top ===")]
    [SerializeField] private Toggle x1Toggle;
    [SerializeField] private Toggle x10Toggle;
    [SerializeField] private Toggle x100Toggle;
    private RectTransform x1Highlight;
    private RectTransform x10Highlight;
    private RectTransform x100Highlight;

    [Header("=== Bottom ===")]
    [SerializeField] private Button attackUpgradeBtn;   // Atk
    [SerializeField] private Button defenceUpgradeBtn;  // Def
    [SerializeField] private Button healthUpgradeBtn;   // Hp
    [SerializeField] private TextMeshProUGUI atkLevelText;
    [SerializeField] private TextMeshProUGUI defLevelText;
    [SerializeField] private TextMeshProUGUI hpLevelText;
    [SerializeField] private TextMeshProUGUI attackIncreaseText;
    [SerializeField] private TextMeshProUGUI defenceIncreaseText;
    [SerializeField] private TextMeshProUGUI healthIncreaseText;
    [SerializeField] private TextMeshProUGUI atkGoldCostText;
    [SerializeField] private TextMeshProUGUI defGoldCostText;
    [SerializeField] private TextMeshProUGUI hpGoldCostText;

    private int upgradeMultiplier => GetMultiplier();
    private int viewTrainingLevel; // 1부터 시작

    public void Init()
    {
        viewTrainingLevel = TrainingManaer.Instance.TrainingLevel;
        
        // Training Level
        minusBtn.onClick.AddListener(OnClickMinus);
        plusBtn.onClick.AddListener(OnClickPlus);
        
        // Top
        x1Toggle.onValueChanged.AddListener(OnChangedToggle);
        x10Toggle.onValueChanged.AddListener(OnChangedToggle);
        x100Toggle.onValueChanged.AddListener(OnChangedToggle);
        
        x1Highlight = x1Toggle.transform.GetChild(1).GetComponent<RectTransform>();
        x10Highlight = x10Toggle.transform.GetChild(1).GetComponent<RectTransform>();
        x100Highlight = x100Toggle.transform.GetChild(1).GetComponent<RectTransform>();
        
        x1Toggle.isOn = true;
        
        // Bottom
        attackUpgradeBtn.onClick.AddListener(OnClickAttackLevelUp);
        defenceUpgradeBtn.onClick.AddListener(OnClickDefenceLevelUp);
        healthUpgradeBtn.onClick.AddListener(OnClickHealthLevelUp);

        TrainingManaer.Instance.OnAttackLevelChanged += _ => RefreshBtn();
        TrainingManaer.Instance.OnDefenceLevelChanged += _ => RefreshBtn();
        TrainingManaer.Instance.OnHealthLevelChanged += _ => RefreshBtn();
        
        SetTrainingLevelText();
        RefreshBtn();
    }

    #region Training Level

    private void OnClickMinus()
    {
        if (viewTrainingLevel == 0) return;
        
        viewTrainingLevel--;
        SetTrainingLevelText();
        RefreshBtn();
    }

    private void OnClickPlus()
    {
        int maxTrainingLevel = TrainingManaer.Instance.MaxTrainingLevel - 1;
        
        if (viewTrainingLevel == maxTrainingLevel) return;

        viewTrainingLevel++;
        SetTrainingLevelText();
        RefreshBtn();
    }

    private void SetTrainingLevelText()
    {
        int maxTrainingLevel = TrainingManaer.Instance.MaxTrainingLevel;

        trainingLevelText.text = $"{viewTrainingLevel + 1}/{maxTrainingLevel}";
    }
    
    #endregion

    #region Top
    
    private int GetMultiplier()
    {
        if (x1Toggle.isOn) return 1;
        if (x10Toggle.isOn) return 10;
        if (x100Toggle.isOn) return 100;
        
        return 1;
    }

    private void OnChangedToggle(bool isOn)
    {
        x1Highlight.gameObject.SetActive(false);
        x10Highlight.gameObject.SetActive(false);
        x100Highlight.gameObject.SetActive(false);

        x1Highlight.DOKill();
        x10Highlight.DOKill();
        x100Highlight.DOKill();

        if (x1Toggle.isOn) PlayHighlight(x1Highlight);
        if (x10Toggle.isOn) PlayHighlight(x10Highlight);
        if (x100Toggle.isOn) PlayHighlight(x100Highlight);
        
        RefreshBtn();
    }

    private void PlayHighlight(RectTransform target)
    {
        target.DOKill();

        target.gameObject.SetActive(true);
        target.sizeDelta = Vector2.one * 10f;
        
        Vector2 endValue = target.sizeDelta + new Vector2(10f, 10f);

        target.DOSizeDelta(endValue, 0.5f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    #endregion

    #region Bottom

    private void OnClickAttackLevelUp()
    {
        TrainingManaer.Instance.LevelUpAttack(upgradeMultiplier);
    }
    
    private void OnClickDefenceLevelUp()
    {
        TrainingManaer.Instance.LevelUpDefence(upgradeMultiplier);
    }
    
    private void OnClickHealthLevelUp()
    {
        TrainingManaer.Instance.LevelUpHealth(upgradeMultiplier);
    }

    private void RefreshBtn()
    {
        //viewTrainingLevel
        bool isViewTrainingLevelMax = viewTrainingLevel == TrainingManaer.Instance.MaxTrainingLevel;
        int maxLevel = TrainingManaer.Instance.GetMaxLevel(viewTrainingLevel);

        // Attack
        int attackLevel = isViewTrainingLevelMax ? maxLevel : TrainingManaer.Instance.AttackLevel;
        int attackIncrease = TrainingManaer.Instance.GetAttackIncrease(viewTrainingLevel, attackLevel);
        long attackUpgradeCost = TrainingManaer.Instance.GetAttackUpgradeCost(viewTrainingLevel, upgradeMultiplier);
        string attackUpgradeCostStr = UIManager.Instance.NumberFormatter(attackUpgradeCost);

        atkLevelText.text = $"Lv.{attackLevel:000}";
        attackIncreaseText.text = $"+{attackIncrease}";
        atkGoldCostText.text = attackUpgradeCostStr;

        // Defence
        int defenceLevel = isViewTrainingLevelMax ? maxLevel : TrainingManaer.Instance.DefenceLevel;
        int defenceIncrease = TrainingManaer.Instance.GetDefenceIncrease(viewTrainingLevel, defenceLevel);
        long defenceUpgradeCost = TrainingManaer.Instance.GetDefenceUpgradeCost(viewTrainingLevel, upgradeMultiplier);
        string defenceUpgradeCostStr = UIManager.Instance.NumberFormatter(defenceUpgradeCost);

        defLevelText.text = $"Lv.{defenceLevel:000}";
        defenceIncreaseText.text = $"+{defenceIncrease}";
        defGoldCostText.text = defenceUpgradeCostStr;

        // Health
        int healthLevel = isViewTrainingLevelMax ? maxLevel : TrainingManaer.Instance.HealthLevel;
        int healthIncrease = TrainingManaer.Instance.GetHealthIncrease(viewTrainingLevel, healthLevel);
        long healthUpgradeCost = TrainingManaer.Instance.GetHealthUpgradeCost(viewTrainingLevel, upgradeMultiplier);
        string healthUpgradeCostStr = UIManager.Instance.NumberFormatter(healthUpgradeCost);

        hpLevelText.text = $"Lv.{healthLevel:000}";
        healthIncreaseText.text = $"+{healthIncrease}";
        hpGoldCostText.text = healthUpgradeCostStr;
    }
    
    #endregion
    
}
