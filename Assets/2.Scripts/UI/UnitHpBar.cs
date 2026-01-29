using System;
using UnityEngine;
using UnityEngine.UI;

public class UnitHpBar : MonoBehaviour
{
    private Canvas canvas;
    private RectTransform canvasRect;
    private RectTransform rect;
    [SerializeField] private Image hpImg;

    public Transform targetUnit;
    public Vector3 offset;

    private void OnEnable()
    {
        SetHpFillAmount(1f);
    }

    public void Init(Transform target)
    {
        canvas = GetComponentInParent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();
        rect = GetComponent<RectTransform>();
        
        targetUnit = target;
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
        if (targetUnit == null) return;
        
        SetPosition(targetUnit.position);
    }

    public void Release()
    {
        targetUnit = null;
        gameObject.SetActive(false);
    }
}