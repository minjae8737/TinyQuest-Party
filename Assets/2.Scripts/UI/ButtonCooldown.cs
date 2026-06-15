using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCoolDown : MonoBehaviour
{
    private Button button;
    [SerializeField] private float delay = 0.3f;

    private Tween delayedCall;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        if (delayedCall != null && delayedCall.IsActive()) return;

        button.interactable = false;

        delayedCall = DOVirtual.DelayedCall(delay, () =>
        {
            button.interactable = true;
        });
    }
}
