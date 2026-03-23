using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class DroppedItem : Poolable
{
    [Header("=== Reference ===")]
    [SerializeField] protected SpriteRenderer sr;
    
    [Header("=== Info ===")]
    private DroppedItemData data;

    public void Init(DroppedItemData data)
    {
        this.data = data;
        sr.sprite = data.Icon;
    }

    private void OnEnable()
    {
        Invoke("OnDrop", 0.2f);
    }

    private void OnDrop()
    {
        transform.DOJump(transform.position + GetDropPos(), 1f, 1, 0.3f)
            .OnComplete(OnPickup);
    }

    private void OnPickup()
    {
        Vector3 targetPos = GetTargetPickupPos();

        transform.DOMove(targetPos, 0.5f)
            .SetEase(Ease.InQuad)
            .OnComplete(OnCompletePickup);
    }

    private void OnCompletePickup()
    {
        data.OnPickup();
        PoolManager.Instance.Release(this);
    }

    private Vector3 GetTargetPickupPos()
    {
        Vector3 targetPos = Vector3.zero;
        
        switch (data)
        {
            case DroppedItem_Currency:
                var currency = (DroppedItem_Currency)data;
                targetPos = UIManager.Instance.GetCurrencyPos(currency.Type);
                break;
            case DroppedItem_Item:
                targetPos = UIManager.Instance.GetInventoryPos();
                break;
        }

        return targetPos;
    }

    private Vector3 GetDropPos()
    {
        float randX = Random.Range(-0.5f, 0.5f);
        return new Vector3(randX, 0f, 0f);
    }
}
