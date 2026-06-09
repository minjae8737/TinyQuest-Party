using UnityEngine;
using UnityEngine.UI;

public class SetupPopup : UniquePopupUI
{
    [SerializeField] private Button closeBtn;

    private void Awake()
    {
        closeBtn.onClick.AddListener(Close);
        closeBtn.onClick.AddListener(() => UIEffect.Punch(closeBtn.transform as RectTransform));
    }
}
