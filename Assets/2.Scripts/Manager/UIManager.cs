using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    [Header("=== Canvas References ===")]
    [SerializeField] private RectTransform worldCanvasRect;

    [SerializeField] private RectTransform damageTextRect;
    public RectTransform DamageTextRect => damageTextRect;
    [SerializeField] private RectTransform canvasRect;
    
    [SerializeField] private GameObject mainButtonGroup;
    
    [SerializeField] private PartySetupPanel partySetupPanel;

    [Header("=== Main Top Panel ===")]
    [SerializeField] private RectTransform GoldPanel;
    [SerializeField] private RectTransform ExpPanel;
    private RectTransform GoldPanelIcon;
    private RectTransform ExpPanelIcon;
    private TextMeshProUGUI goldText;
    private TextMeshProUGUI expText;

    [Header("=== Training Panel ===")] 
    [SerializeField] private TrainingPanel trainingPanel;

    [Header("=== MainQuest Panel ===")] 
    [SerializeField] private MainQuestPanel mainQuestPanel;

    [Header("=== Prefab ===")] 
    [SerializeField] private GameObject DragItemUIPrefab;

    private DragItemUI DragItemUI;
    [HideInInspector] public DragContext DragContext;
    private bool isDragged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Init()
    {
        isDragged = false;
        
        // DragItemUI
        GameObject dragItemObj = Instantiate(DragItemUIPrefab, canvasRect);
        
        if (!dragItemObj.TryGetComponent<DragItemUI>(out var dragItem))
        {
            Debug.LogError("Fail Create DragItemUI");
        }
        
        DragItemUI = dragItem;
        DragItemUI.SetActive(false);
        
        // MainButtonGroup
        mainButtonGroup.SetActive(true);
        
        // PartySetupPanel
        partySetupPanel.Init();
        
        // Main Top Panel
        GoldPanelIcon = GoldPanel.GetChild(0).GetComponent<RectTransform>();
        ExpPanelIcon = ExpPanel.GetChild(0).GetComponent<RectTransform>();
        goldText = GoldPanel.GetChild(1).GetComponent<TextMeshProUGUI>();
        expText = ExpPanel.GetChild(1).GetComponent<TextMeshProUGUI>();
        
        // Training Panel
        trainingPanel.Init();
        
        // Main Quest Panel
        mainQuestPanel.Init();
        
        RefreshGoldPanel();
        RefreshExpPanel();
    }

    #region Util
    
    public string NumberFormatter(double value)
    {
        if (value >= 1_000_000_000_000)
            return $"{(value / 1_000_000_000_000):0.#}T";
        if (value >= 1_000_000_000)
            return $"{(value / 1_000_000_000):0.#}B";
        if (value >= 1_000_000)
            return $"{(value / 1_000_000):0.#}M";
        if (value >= 1_000)
            return $"{(value / 1_000):0.#}K";
        
        return $"{value}";
    }

    #endregion

    #region PartySetupPanel

    public void OpenPartySetupPanel()
    {
        partySetupPanel.gameObject.SetActive(true);
        AudioManager.Instance.PlaySfx(Sfx.UIOpen);
    }

    public void OffPartySetupPanel()
    {
        partySetupPanel.gameObject.SetActive(false);
        AudioManager.Instance.PlaySfx(Sfx.UIClose);
    }

    #endregion

    #region Drag
    
    public DragItemUI GetDragItem()
    {
        return DragItemUI;
    }

    #endregion

    // MainTopGroup 임시 네이밍
    #region MainTopGroup

    public void RefreshGoldPanel()
    {
        long gold = CurrencyManager.Instance.Gold;
        goldText.text = NumberFormatter(gold);
    }

    public void RefreshExpPanel()
    {
        long exp = CurrencyManager.Instance.Exp;
        expText.text = NumberFormatter(exp);
    }
    
    public Vector3 GetCurrencyPos(CurrencyType type)
    {
        Vector3 screenPos = Vector3.zero;
        
        switch (type)
        {
            case CurrencyType.Gold:
                screenPos= RectTransformUtility.WorldToScreenPoint(null, GoldPanelIcon.position);
                break;
            case CurrencyType.Exp:
                screenPos= RectTransformUtility.WorldToScreenPoint(null, ExpPanelIcon.position);
                break;
        }

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        worldPos.z = 0f;

        return worldPos;
    }

    public Vector3 GetInventoryPos()
    {
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(null, GoldPanelIcon.position); //TODO GoldPanelIcon 을 inventoryButton으로 바꿀 것 
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        worldPos.z = 0f;

        return worldPos;
    }

    #endregion

    #region TrainingPanel

    public void OpenTrainingPanel()
    {
        trainingPanel.gameObject.SetActive(true);
        AudioManager.Instance.PlaySfx(Sfx.UIOpen);
    }

    public void OffTrainingPanel()
    {
        trainingPanel.gameObject.SetActive(false);
        AudioManager.Instance.PlaySfx(Sfx.UIClose);
    }

    #endregion

    #region MainQuestPanel

    public void OpenMainQuestPanel()
    {
        if (mainQuestPanel.gameObject.activeSelf) return;
        mainQuestPanel.gameObject.SetActive(true);
    }

    public void OffMainQuestPanel()
    {
        if (!mainQuestPanel.gameObject.activeSelf) return;
        mainQuestPanel.gameObject.SetActive(false);
    }

    #endregion
}
