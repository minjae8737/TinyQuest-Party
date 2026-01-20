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
        SetHp(1f);
    }

    public void Init(Transform target)
    {
        canvas = GetComponentInParent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();
        rect = GetComponent<RectTransform>();
        
        targetUnit = target;
    }
    
    private void SetHp(float fill)
    {
        hpImg.fillAmount = fill;
    }

    private void SetPosition(Vector3 unitPos)
    {
        Vector2 worldToScreenPoint = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, unitPos + offset);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, worldToScreenPoint, canvas.worldCamera, out Vector2 localPoint);

        rect.anchoredPosition = localPoint;
    }

    private void LateUpdate()
    {
        SetPosition(targetUnit.position);
    }
}