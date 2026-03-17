using System;
using UnityEngine;
using UnityEngine.UI;

public class UnitHpBar : MonoBehaviour
{
    [Header("=== UI Reference ===")]
    [SerializeField] private Image hpImg;
    
    [Header("=== Follow Settings ===")]
    [SerializeField] private Vector3 offset;
    
    private Canvas canvas;
    private RectTransform canvasRect;
    private RectTransform rect;

    public Transform TargetUnit { get; private set; }

    private void OnEnable()
    {
        SetHpFillAmount(1f);
    }

    public void Init(Transform target)
    {
        canvas = GetComponentInParent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();
        rect = GetComponent<RectTransform>();
        
        TargetUnit = target;
        gameObject.SetActive(true);
    }
    
    private void SetHpFillAmount(float fill)
    {
        hpImg.fillAmount = fill;
    }

    public void SetHp(float maxHp, float hp)
    {
        float fill = maxHp == 0 ? 0 : hp / maxHp;
        SetHpFillAmount(fill);
    }

    private void SetPosition(Vector3 unitPos)
    {
        Vector2 worldToScreenPoint = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, unitPos + offset);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, worldToScreenPoint, canvas.worldCamera, out Vector2 localPoint);

        rect.anchoredPosition = localPoint;
    }

    private void LateUpdate()
    {
        if (TargetUnit == null) return;
        
        SetPosition(TargetUnit.position);
    }

    public void Release()
    {
        TargetUnit = null;
        UnitManager.Instance.ReleaseUnitHpBar(this);
    }
}