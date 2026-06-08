using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupUI : Poolable
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private Button confirmBtn;
    [SerializeField] private TMP_Text confirmBtnText;

    private void Awake()
    {
        confirmBtn.onClick.AddListener(OnClickConfirmButton);
    }

    public virtual void Show(string title, string message, string confirm)
    {
        titleText.text = title;
        messageText.text = message;
        confirmBtnText.text = confirm;
        
        Open();
    }
    
    public virtual void Open() { UIEffect.OpenPopup(transform as RectTransform); }

    public virtual void Close()
    {
        UIEffect.ClosePopup(
            transform as RectTransform,
            () => PopupManager.Instance.HidePopup(this)
            );
    }
    
    public virtual void OnClickConfirmButton() { }
}
