using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmPopupUI : PooledPopupUI
{
    [SerializeField] protected TMP_Text titleText;
    [SerializeField] protected TMP_Text messageText;
    [SerializeField] protected Button confirmBtn;
    [SerializeField] protected TMP_Text confirmBtnText;

    private void Awake()
    {
        confirmBtn.onClick.AddListener(Close);
    }
    
    public override void Show()
    {
        Open();
    }
    
    public void Setup(string title, string message, string confirm)
    {
        titleText.text = title;
        messageText.text = message;
        confirmBtnText.text = confirm;
    }
}
