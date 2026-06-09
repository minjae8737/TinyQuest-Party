using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccountPopup : UniquePopupUI
{
    [SerializeField] private TMP_InputField emaillInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TMP_Text errorText;

    [SerializeField] private GameObject linkedEmailObj;
    [SerializeField] private GameObject unLinkedEmailObj;
    [SerializeField] private TMP_Text linkedEmailText;
    
    [SerializeField] private Button loginBtn;
    [SerializeField] private Button closeBtn;
    
    private void Awake()
    {
        closeBtn.onClick.AddListener(Close);
        closeBtn.onClick.AddListener(() => UIEffect.Punch(closeBtn.transform as RectTransform));

        loginBtn.onClick.AddListener(OnClickLoginBtn);
        loginBtn.onClick.AddListener(() => UIEffect.Punch(loginBtn.transform as RectTransform));
    }

    public override void Show()
    {
        base.Show();
        RefreshUI();
    }

    private void RefreshUI()
    {
        bool isLinked = FirebaseAuthManager.Instance.IsEmailLinked;

        linkedEmailObj.SetActive(isLinked);
        unLinkedEmailObj.SetActive(!isLinked);

        if (isLinked)
            linkedEmailText.text = $"연동된 계정: {FirebaseAuthManager.Instance.CurrentUser.Email}";
    }

    private async void OnClickLoginBtn()
    {
        try
        {
            await FirebaseAuthManager.Instance.LinkWithEmail(emaillInputField.text, passwordInputField.text);
            RefreshUI();
        }
        catch (Exception e)
        {
            PopupManager.Instance.ShowConfirm(
                title: "알림",
                message: e.Message,
                confirm: "확인"
            );
        }
    }
}
