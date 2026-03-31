using DG.Tweening;
using TMPro;
using UnityEngine;

public class FloatingText : Poolable
{
    [Header("=== Reference ===")]
    [SerializeField] private TextMeshProUGUI text;

    [Header("=== Reference ===")]
    private string content;
    private float delay = 0f;
    private float duration;

    private Tween tween;

    private void OnEnable()
    {
        Invoke("Play", delay);
    }

    public void Init(string content, float duration, float delay = 0f)
    {
        text.text = content;
        this.duration = duration;
        this.delay = delay;
    }

    private void Play()
    {
        float originPosY = transform.position.y;

        transform.DOMoveY(originPosY + 0.7f, duration).OnComplete(OnComplete);
    }

    private void OnComplete()
    {
        PoolManager.Instance.Release(this);
    }

}
