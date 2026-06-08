using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void ShowConfirmPopup(string title, string message, string confirm)
    {
        ConfirmPopupUI popup = PoolManager.Instance.Get<ConfirmPopupUI>();
        popup.Show(
            title: title,
            message: message,
            confirm: confirm
        );
    }
    
}
