using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; }

    [SerializeField] private GameObject popupBackGround;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    public void ShowConfirmPopup(string title, string message, string confirm)
    {
        if (!popupBackGround.activeSelf) 
            popupBackGround.SetActive(true);
        
        ConfirmPopupUI popup = PoolManager.Instance.Get<ConfirmPopupUI>();
        popup.Show(
            title: title,
            message: message,
            confirm: confirm
        );
    }

    public void HidePopup(PopupUI popup)
    {
        if (popupBackGround.activeSelf) 
            popupBackGround.SetActive(false);
        
        PoolManager.Instance.Release(popup);
    }
    
}
