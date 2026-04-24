using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class RewardEffect
{
    [SerializeField] private float scatterDuration = 0.4f; // 동전 퍼트릴때
    [SerializeField] private float scatterRadius = 150f; // 동전 퍼트릴때 범위
    [SerializeField] private float waitBeforeFly = 0.2f;  // 퍼트린 후 대기 시간
    [SerializeField] private float randDelay = 0.2f;  // 딜레이 시간 범위
    [SerializeField] private float flyDuration = 0.7f; // 동전 날아갈때

    public void PlayEffect(int count, Sprite sprite, RectTransform start, RectTransform target, Action onComplete = null)
    {
        List<RectTransform> items = new();
        List<RewardEffectItem> rewardEffectItems = new();

        // 생성
        for (int i = 0; i < count; i++)
        {
            RewardEffectItem item = PoolManager.Instance.Get<RewardEffectItem>();
            item.Init(sprite);
            rewardEffectItems.Add(item);
            
            RectTransform itemRect = item.GetComponent<RectTransform>();
            itemRect.anchoredPosition = start.anchoredPosition;
            
            items.Add(itemRect);
        }
        
        // 랜덤으로 퍼뜨리기
        foreach (RectTransform item in items)
        {
            // 퍼트리는 위치
            Vector2 randOffset = Random.insideUnitCircle.normalized * Random.Range(scatterRadius * 0.5f, scatterRadius);
            item.DOAnchorPos(item.anchoredPosition + randOffset, scatterDuration).SetEase(Ease.OutCubic);
            
            // 회전
            float randAngle = Random.Range(-45f, 45f);
            item.DORotate(new Vector3(0, 0, randAngle), scatterDuration).SetEase(Ease.OutCubic);
        }
        
        // 날아가기
        for (int i = 0; i < items.Count; i++)
        {           
            float randDelay = Random.Range(0, 0.5f);
           
            int idx = i;
            var item = items[i];
            
            DOVirtual.DelayedCall(randDelay, () =>
            {
                item.DOMove(target.position, flyDuration)
                    .SetEase(Ease.InBack)
                    .SetDelay(waitBeforeFly) // 날아가기전 잠깐 멈춤
                    .OnComplete(() =>
                    {
                        // 반환
                        PoolManager.Instance.Release(rewardEffectItems[idx]);
                        onComplete?.Invoke();
                    });

                // 날아가면서 회전 초기화
                item.DORotate(Vector3.zero, flyDuration * 0.5f);
                
                // 날아가면서 살짝 커졌다 작아짐
                item.DOScale(1.2f, flyDuration * 0.3f)
                    .OnComplete(() => item.DOScale(0.8f, flyDuration * 0.4f));
            });
        }
    }
}
