using UnityEngine;

public class SafeArea : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    Rect lastSafeArea;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        Apply();
    }
    
    #if UNITY_EDITOR
    
    void Update()
    {
        if (lastSafeArea != Screen.safeArea)
            Apply();
    }
    
    #endif
    
    private void Apply()
    {
        Rect safeArea = Screen.safeArea;
        lastSafeArea = safeArea;

        // Safe Area를 0~1 비율로 변환
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        
        // offset 초기화
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }
}
