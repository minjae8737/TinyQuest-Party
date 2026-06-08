using System.Collections.Generic;
using UnityEngine;

public class PopupManager : Singleton<PopupManager>
{
    [SerializeField] private SetupPopup setupPopup;
    [SerializeField] private AccountPopup accountPopup;
    
    [SerializeField] private GameObject popupBackGround;
    
    private readonly Stack<PopupUI> popupStack = new();

    public void ShowSetup()  => Show(setupPopup);
    public void ShowAccount() => Show(accountPopup);

    public void Show(UniquePopupUI popup)
    {
        popupStack.Push(popup);
        popup.Show();
    }

    public T ShowPooled<T>() where T : PooledPopupUI
    {
        if (!popupBackGround.activeSelf)
            popupBackGround.SetActive(true);
        
        T popup = PoolManager.Instance.Get<T>();
        popupStack.Push(popup);
        popup.Show();
        return popup;
    }

    public void ShowConfirm(string title, string message, string confirm)
    {
        var popup = ShowPooled<ConfirmPopupUI>();
        popup.Setup(title, message, confirm);
    }

    public void HidePopup(PopupUI popup)
    {
        popup.OnHide();

        if (popupStack.Count > 0 && popupStack.Peek() == popup)
            popupStack.Pop();

        // Pooled 팝업이면 반납
        if (popup is PooledPopupUI pooled)
        {
            // 남은 스택에 Pooled 팝업이 없으면 공용 배경 끔
            if (!HasAnyPooledPopup())
                popupBackGround.SetActive(false);

            PoolManager.Instance.Release(pooled);
        }
        
        if (popupBackGround.activeSelf) 
            popupBackGround.SetActive(false);
        
        // PoolManager.Instance.Release(popup);
    }
    
    private bool HasAnyPooledPopup()
    {
        foreach (var p in popupStack)
        {
            if (p is PooledPopupUI) return true;
        }
        
        return false;
    }
    
}
