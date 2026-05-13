using UnityEngine;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image btnImage;

    [SerializeField] private UIPage targetPage;

    [Header("=== Resource ===")] 
    [SerializeField] private Sprite btnIcon;
    [SerializeField] private Sprite backIcon;

    private bool isOn;

    public UIPage TargetPage => targetPage;
    public bool IsOn => isOn;
    
    
    public void Init()
    {
        button.onClick.AddListener(OnClicked);
        isOn = false;
    }
    
    private void OnClicked()
    {
        UIEffect.Punch(button.transform as RectTransform);
        UIManager.Instance.OnMainButtonClicked(this);
    }

    public void SetState(bool isOn)
    {
        this.isOn = isOn;
        btnImage.sprite = this.isOn ? backIcon : btnIcon;
    }
}
