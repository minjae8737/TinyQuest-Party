using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageHUD : MonoBehaviour
{
    [Header("=== Reference ===")]
    [SerializeField] private TMP_Text stageText;
    [SerializeField] private TMP_Text roundText;
    [SerializeField] private Image progressFill;
    [SerializeField] private RectTransform progressIcon;

    #region RunTime
    
    private float fillWidth;
    private float originIconXPos;

    private Tween progressTween;
    private Tween iconMoveTween;
    private Sequence noddingSequence;
    
    #endregion
    
    public void Init()
    {
        fillWidth = progressFill.rectTransform.sizeDelta.x;
        originIconXPos = fillWidth / 2;
        
        RefreshStageText();
        RefreshRoundText();
        RefreshProgress();
        
        NoddingLoop(progressIcon);

        StageManager.Instance.OnStageChanged += RefreshStageText;
        StageManager.Instance.OnChangedIsland += RefreshRoundText;
        StageManager.Instance.OnChangedProgress += RefreshProgress;
    }

    #region Setting

    private void RefreshStageText()
    {
        stageText.text = $"Stage {StageManager.Instance.CurStageLevel + 1}";
    }

    private void RefreshRoundText()
    {
        roundText.text = $"ROUND {StageManager.Instance.CurIslandIdx + 1} / {StageManager.Instance.CurIslandCount}";
    }

    private void RefreshProgress()
    {
        float stageProgress = StageManager.Instance.GetStageProgress();
        float xPos = originIconXPos - fillWidth * (1f - stageProgress);
        
        MoveFillAmount(progressFill, stageProgress);
        MoveAnchoredPosX(progressIcon, xPos);
    }

    #endregion

    #region UIEffect
    
    private void NoddingLoop(RectTransform rect)
    {
        noddingSequence?.Kill();
        noddingSequence = DOTween.Sequence();

        noddingSequence.Append(rect.DORotate(new Vector3(0, 0, -12f), 0.3f))
            .SetEase(Ease.InOutQuad);
        
        noddingSequence.Join(rect.DOAnchorPosY(rect.anchoredPosition.y + 6f, 0.5f))
            .SetEase(Ease.InOutQuad);

        noddingSequence.SetLoops(-1, LoopType.Yoyo);
    }
    
    private void MoveAnchoredPosX(RectTransform rect, float endValue)
    {
        progressTween?.Kill();
        progressTween = rect.DOAnchorPosX(endValue, 0.3f).SetEase(Ease.OutCubic);
    }

    private void MoveFillAmount(Image image, float endValue)
    {
        iconMoveTween?.Kill();
        iconMoveTween = image.DOFillAmount(endValue, 0.3f).SetEase(Ease.OutCubic);
    }

    #endregion

}
