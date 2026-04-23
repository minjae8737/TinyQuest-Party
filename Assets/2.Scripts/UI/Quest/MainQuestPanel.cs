using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainQuestPanel : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI desc;
    [SerializeField] private Image rewardIcon;
    [SerializeField] private TextMeshProUGUI rewardAmount;
    [SerializeField] private RectTransform highlightRect;
    [SerializeField] private Button highlightBtn;

    [Header("=== Resource ===")] 
    [SerializeField] private Sprite goldIcon; 
    [SerializeField] private Sprite expIcon; 

    public void Init()
    {
        QuestManager.Instance.OnMainQuestClear += OnClearQuest;
        QuestManager.Instance.OnMainQuestRewardProvided += OnProvideReward;
        QuestManager.Instance.OnMainQuestUpdated += RefreshPanel;
        
        RefreshPanel();
        
        StopHighlight(highlightRect);
    }

    private void RefreshPanel()
    {
        desc.text = QuestManager.Instance.GetMainQuestDesc();
        //TODO 나중에 보상 종류가 늘어날수 있게 구조 고치기
        rewardIcon.sprite = QuestManager.Instance.GetRewardType() == RewardType.Gold ? goldIcon : expIcon;
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
        highlightBtn.onClick.RemoveAllListeners();
        QuestManager.Instance.ProvideReward();
    }

    private void OnProvideReward()
    {
        highlightBtn.onClick.RemoveAllListeners();
        StopHighlight(highlightRect);
        
    }

}
