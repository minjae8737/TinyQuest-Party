using UnityEngine;

public class PopupUI : Poolable
{
    [SerializeField] protected GameObject background;
    protected virtual bool HasBackground => false;

    public virtual void Show()
    {
        if (HasBackground && background != null)
            background.SetActive(true);
        
        Open();
    }

    public virtual void OnHide()
    {
        if (HasBackground && background != null)
            background.SetActive(false);
    }

    public virtual void Open() { UIEffect.OpenPopup(transform as RectTransform); }

    public virtual void Close()
    {
        UIEffect.ClosePopup(
            transform as RectTransform,
            () => PopupManager.Instance.HidePopup(this)
            );
    }
}
