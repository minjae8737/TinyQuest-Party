using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainQuestPanel : UIPage
{
    
    [SerializeField] private TextMeshProUGUI desc;
    [SerializeField] private Image rewardIcon;
    [SerializeField] private TextMeshProUGUI rewardAmount;
    [SerializeField] private RectTransform highlightRect;
    [SerializeField] private Button highlightBtn;

    [Header("=== Resource ===")] 
    [SerializeField] private QuestRewardConfig rewardConfig;

    [Header("=== Effect ===")] 
    [SerializeField] private RewardEffect rewardEffect;

    public void Init()
    {
        rewardConfig.Init();
        QuestManager.Instance.OnMainQuestClear += OnClearQuest;
        QuestManager.Instance.OnMainQuestRewardProvided += OnProvideReward;
        QuestManager.Instance.OnMainQuestUpdated += RefreshPanel;
        
        RefreshPanel();
        
        StopHighlight(highlightRect);
    }
    
    public override void Show()
    {
        if (gameObject.activeSelf) return;
        gameObject.SetActive(true);
    }

    public override void Hide()
    {
        if (!gameObject.activeSelf) return;
        gameObject.SetActive(false);
    }

    private void RefreshPanel()
    {
        QuestReward rewardData = QuestManager.Instance.GetRewardData();
        
        desc.text = QuestManager.Instance.GetMainQuestDesc();
        rewardIcon.sprite = rewardConfig.GetIcon(rewardData.Type); 
    }
    
    private void PlayHighlight(RectTransform target)
    {
        target.DOKill();

        target.gameObject.SetActive(true);
        target.sizeDelta = Vector2.one * 130f;
        
        Vector2 endValue = target.sizeDelta + new Vector2(10f, 10f);

        target.DOSizeDelta(endValue, 0.5f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    private void StopHighlight(RectTransform target)
    {
        target.DOKill();
        target.gameObject.SetActive(false);
    }

    private void OnClearQuest()
    {
        PlayHighlight(highlightRect);  
        
        highlightBtn.onClick.RemoveAllListeners();
        highlightBtn.onClick.AddListener(OnClickHighlightBtn);
    }

    private void OnClickHighlightBtn()
    {
        QuestReward rewardData = QuestManager.Instance.GetRewardData();
        Sprite icon = rewardConfig.GetIcon(rewardData.Type);
        RectTransform panelIconRect = UIManager.Instance.GetPanelIcon(rewardData.Type);
        
        rewardEffect.PlayEffect(10, icon, highlightRect, panelIconRect);
        
        highlightBtn.onClick.RemoveAllListeners();
        QuestManager.Instance.ProvideReward();
    }

    private void OnProvideReward()
    {
        highlightBtn.onClick.RemoveAllListeners();
        StopHighlight(highlightRect);
        
    }

}
