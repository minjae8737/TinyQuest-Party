using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrainingPanel : UIPage
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
        viewTrainingLevel = TrainingManager.Instance.TrainingLevel;
        
        // Training Level
        minusBtn.onClick.AddListener(OnClickMinus);
        plusBtn.onClick.AddListener(OnClickPlus);
        
        // Top
        x1Toggle.onValueChanged.AddListener(OnChangedToggle);
        x10Toggle.onValueChanged.AddListener(OnChangedToggle);
        x100Toggle.onValueChanged.AddListener(OnChangedToggle);
        x1Toggle.onValueChanged.AddListener(_ => AudioManager.Instance.PlaySfx(Sfx.ChangeToggle));
        x10Toggle.onValueChanged.AddListener(_ => AudioManager.Instance.PlaySfx(Sfx.ChangeToggle));
        x100Toggle.onValueChanged.AddListener(_ => AudioManager.Instance.PlaySfx(Sfx.ChangeToggle));
        
        x1Highlight = x1Toggle.transform.GetChild(1).GetComponent<RectTransform>();
        x10Highlight = x10Toggle.transform.GetChild(1).GetComponent<RectTransform>();
        x100Highlight = x100Toggle.transform.GetChild(1).GetComponent<RectTransform>();
        
        x1Toggle.isOn = true;
        
        // Bottom
        attackUpgradeBtn.onClick.AddListener(OnClickAttackLevelUp);
        defenceUpgradeBtn.onClick.AddListener(OnClickDefenceLevelUp);
        healthUpgradeBtn.onClick.AddListener(OnClickHealthLevelUp);
        
        TrainingManager.Instance.OnTrainingLevelChanged += HandleLevelUpTrainingLevel;
        TrainingManager.Instance.OnAttackLevelChanged += _ => RefreshBtn();
        TrainingManager.Instance.OnDefenceLevelChanged += _ => RefreshBtn();
        TrainingManager.Instance.OnHealthLevelChanged += _ => RefreshBtn();
        
        SetTrainingLevelText();
        OnChangedToggle(false);
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
        if (viewTrainingLevel == TrainingManager.Instance.TrainingLevel) return;

        viewTrainingLevel++;
        SetTrainingLevelText();
        RefreshBtn();
    }

    private void SetTrainingLevelText()
    {
        int maxTrainingLevel = TrainingManager.Instance.MaxTrainingLevel;

        trainingLevelText.text = $"{viewTrainingLevel + 1}/{maxTrainingLevel}";
    }

    private void HandleLevelUpTrainingLevel()
    {
        viewTrainingLevel = TrainingManager.Instance.TrainingLevel;
        SetTrainingLevelText();
        RefreshBtn();
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
        TrainingManager.Instance.LevelUpStat(TrainingType.Attack, upgradeMultiplier);
    }
    
    private void OnClickDefenceLevelUp()
    {
        TrainingManager.Instance.LevelUpStat(TrainingType.Defence, upgradeMultiplier);
    }
    
    private void OnClickHealthLevelUp()
    {
        TrainingManager.Instance.LevelUpStat(TrainingType.Health, upgradeMultiplier);
    }

    private void RefreshBtn()
    {
        //viewTrainingLevel
        bool lessViewTrainingLevel = viewTrainingLevel < TrainingManager.Instance.TrainingLevel;
        int maxLevel = TrainingManager.Instance.GetMaxLevel(viewTrainingLevel);

        // Attack
        int attackLevel = lessViewTrainingLevel ? maxLevel : TrainingManager.Instance.AttackLevel;
        int attackIncrease = TrainingManager.Instance.GetIncrease(TrainingType.Attack, viewTrainingLevel, attackLevel);
        long attackUpgradeCost = TrainingManager.Instance.GetUpgradeCost(TrainingType.Attack, viewTrainingLevel, upgradeMultiplier);
        string attackUpgradeCostStr = UIManager.Instance.NumberFormatter(attackUpgradeCost);

        atkLevelText.text = $"Lv.{attackLevel:000}";
        attackIncreaseText.text = $"+{attackIncrease}";
        atkGoldCostText.text = attackUpgradeCostStr;
        attackUpgradeBtn.interactable = attackUpgradeCost != 0L; 

        // Defence
        int defenceLevel = lessViewTrainingLevel ? maxLevel : TrainingManager.Instance.DefenceLevel;
        int defenceIncrease = TrainingManager.Instance.GetIncrease(TrainingType.Defence, viewTrainingLevel, defenceLevel);
        long defenceUpgradeCost = TrainingManager.Instance.GetUpgradeCost(TrainingType.Defence, viewTrainingLevel, upgradeMultiplier);
        string defenceUpgradeCostStr = UIManager.Instance.NumberFormatter(defenceUpgradeCost);

        defLevelText.text = $"Lv.{defenceLevel:000}";
        defenceIncreaseText.text = $"+{defenceIncrease}";
        defGoldCostText.text = defenceUpgradeCostStr;
        defenceUpgradeBtn.interactable = defenceUpgradeCost != 0L;

        // Health
        int healthLevel = lessViewTrainingLevel ? maxLevel : TrainingManager.Instance.HealthLevel;
        int healthIncrease = TrainingManager.Instance.GetIncrease(TrainingType.Health, viewTrainingLevel, healthLevel);
        long healthUpgradeCost = TrainingManager.Instance.GetUpgradeCost(TrainingType.Health, viewTrainingLevel, upgradeMultiplier);
        string healthUpgradeCostStr = UIManager.Instance.NumberFormatter(healthUpgradeCost);

        hpLevelText.text = $"Lv.{healthLevel:000}";
        healthIncreaseText.text = $"+{healthIncrease}";
        hpGoldCostText.text = healthUpgradeCostStr;
        healthUpgradeBtn.interactable = healthUpgradeCost != 0L;
    }
    
    #endregion
    
}
