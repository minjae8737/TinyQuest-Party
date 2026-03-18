using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
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
        transform.DOMoveY(0.7f, duration).OnComplete(OnComplete);
    }

    private void OnComplete()
    {
        PoolManager.Instance.Release(gameObject);
    }

}
