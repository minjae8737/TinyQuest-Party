using System;

public class UnitInfoCardUI : UnitCardUI
{
    public event Action<UnitName> OnClicked; 
    
    protected override void OnClickCard()
    {
        OnClicked?.Invoke(UnitName);
    }
}
