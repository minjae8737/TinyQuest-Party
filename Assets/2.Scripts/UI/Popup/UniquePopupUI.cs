using UnityEngine;

public class UniquePopupUI : PopupUI
{
    [SerializeField] private RectTransform popupRect;
    protected override bool HasBackground => true;

    public override void Show()
    {
        if (HasBackground && background != null)
        {
            background.SetActive(true);
        }
        
        transform.gameObject.SetActive(true);
        Open();
    }

    public override void OnHide()
    {
        if (HasBackground && background != null)
            background.SetActive(false);
        
        transform.gameObject.SetActive(false);
    }

    public override void Open()
    {
        UIEffect.OpenPopup(popupRect);
    }

    public override void Close()
    {
        UIEffect.ClosePopup(
            popupRect,
            () => PopupManager.Instance.HidePopup(this)
        );
    }
}
