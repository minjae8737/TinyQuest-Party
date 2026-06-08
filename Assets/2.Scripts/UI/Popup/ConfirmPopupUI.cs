using UnityEngine;

public class ConfirmPopupUI : PopupUI
{
    public override void OnClickConfirmButton()
    {
        Close();
    }
}
