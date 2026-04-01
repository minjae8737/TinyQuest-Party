using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonEffect : MonoBehaviour
{
    [SerializeField] private float punchScale = 0.2f;
    [SerializeField] private float duration = 0.2f;
    
    private Button btn;
    private RectTransform rect;

    private Vector3 originScale;

    private void Awake()
    {
        btn = GetComponent<Button>();
        rect = GetComponent<RectTransform>();

        originScale = rect.localScale;

        btn.onClick.AddListener(PlayEffect);
    }

    private void PlayEffect()
    {
        rect.DOKill(true);
        rect.localScale = originScale;

        rect.DOPunchScale(originScale * punchScale, duration);
    }

}
