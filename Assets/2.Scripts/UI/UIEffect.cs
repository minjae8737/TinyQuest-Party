using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class UIEffect
{
    public static void Punch(RectTransform rect)
    {
        rect.DOKill();
        rect.localScale = Vector3.one;
        rect.DOPunchScale(Vector3.one * 0.2f, 0.3f, 5);
    }

    public static void SlideUp(RectTransform rect, Vector2 targetPos, float offset = 30f, float duration = 0.4f)
    {
        rect.DOKill();
        rect.anchoredPosition = targetPos + Vector2.down * offset;
        rect.DOAnchorPosY(targetPos.y, duration).SetEase(Ease.OutCubic);
    }
    
    public static void CounterTo(TMP_Text text, long start, long end, float duration = 1.5f)
    {
        long value = start;

        DOTween.To(() => value, x =>
        {
            value = x;
            text.text = UIManager.Instance.NumberFormatter(x);
        }, end, duration);
    }
    
    public static void PunchLoop(RectTransform rect)
    {
        rect.DOKill();
        
        Vector2 endValue = rect.sizeDelta + new Vector2(10f, 10f);

        rect.DOSizeDelta(endValue, 0.5f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    public static void Scrolling(RectTransform rect, Vector2 endValue)
    {
        rect.DOKill();
        rect.DOAnchorPos(endValue, 0.7f).SetEase(Ease.OutQuint);
    }

    public static void FadeIn(CanvasGroup group)
    {
        group.DOKill();
        
        group.gameObject.SetActive(true);
        group.alpha = 0f;
        group.interactable = true;
        group.blocksRaycasts = true; 
        
        group.DOFade(1f, 0.5f).SetEase(Ease.OutQuad);
    }

    public static void FadeOut(CanvasGroup group)
    {
        group.DOKill();
        
        group.alpha = 1f;
        group.interactable = false;
        group.blocksRaycasts = false;

        group.DOFade(0f, 0.5f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                group.gameObject.SetActive(false);
            });
    }
    
    public static void StartShakeLoop(RectTransform rect)
    {
        rect.DOKill();

        Vector2 originPos = rect.anchoredPosition;
        
        rect.DOShakeAnchorPos(0.06f, new Vector2(5f, 5f * 0.2f), 10, 45f)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart)
            .OnKill(() =>
            {
                rect.anchoredPosition = originPos;
            });
    }

    public static void StopShakeLoop(RectTransform rect)
    {
        rect?.DOKill();
    }
}
