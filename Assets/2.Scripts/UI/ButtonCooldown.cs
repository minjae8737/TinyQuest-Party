using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCoolDown : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private float delay = 0.3f;

    private Tween delayedCall;
    
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
