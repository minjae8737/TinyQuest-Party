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
}
