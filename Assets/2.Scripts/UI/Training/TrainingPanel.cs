using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TrainingPanel : MonoBehaviour
{
    [Header("=== Top ===")]
    [SerializeField] private Toggle x1Toggle;
    [SerializeField] private Toggle x10Toggle;
    [SerializeField] private Toggle x100Toggle;
    private RectTransform x1Highlight;
    private RectTransform x10Highlight;
    private RectTransform x100Highlight;

    [Header("=== Bottom ===")]
    [SerializeField] private Button attackUpgradeBtn;
    [SerializeField] private Button defenceUpgradeBtn;
    [SerializeField] private Button healthUpgradeBtn;

    private int upgradeMultiplier => GetMultiplier();

    public event Action OnUgradeStepChanged;
    

    public void Init()
    {
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
    }

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


    private void OnClickAttackLevelUp()
    {
        
    }
    
    private void OnClickDefenceLevelUp()
    {
        
    }
    
    private void OnClickHealthLevelUp()
    {
        
    }
    
}
