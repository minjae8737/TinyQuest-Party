using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    private UIPage currentPage;
    
    [Header("=== Canvas References ===")]
    [SerializeField] private RectTransform worldCanvasRect;

    [SerializeField] private RectTransform damageTextRect;
    public RectTransform DamageTextRect => damageTextRect;
    [SerializeField] private RectTransform canvasRect;
    
    
    [SerializeField] private RectTransform dragItemUIParent;

    [Header("=== Main Top Panel ===")]
    [SerializeField] private RectTransform GoldPanel;
    [SerializeField] private RectTransform ExpPanel;
    private RectTransform GoldPanelIcon;
    private RectTransform ExpPanelIcon;
    private TextMeshProUGUI goldText;
    private TextMeshProUGUI expText;
    
    
    [Header("=== MainButtonGroup ===")] 
    [SerializeField] private GameObject mainButtonGroup;
    [SerializeField]private List<Button> mainButtonList;

    [Header("=== PartySetup Panel ===")] 
    [SerializeField] private PartySetupPanel partySetupPanel;
    
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
        currentPage = null;
        isDragged = false;
        
        // DragItemUI
        GameObject dragItemObj = Instantiate(DragItemUIPrefab, dragItemUIParent);
        
        if (!dragItemObj.TryGetComponent<DragItemUI>(out var dragItem))
        {
            Debug.LogError("Fail Create DragItemUI");
        }
        
        DragItemUI = dragItem;
        DragItemUI.SetActive(false);
        
        // MainButtonGroup
        mainButtonGroup.SetActive(true);
        Button[] mainButtons = mainButtonGroup.GetComponentsInChildren<Button>();
        mainButtonList = new(mainButtons);
        foreach (Button button in mainButtons)
        {
            button.onClick.AddListener(()=> UIEffect.Punch(button.transform as RectTransform));
        }

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
        
        RefreshGoldPanel(0);
        RefreshExpPanel(0);
    }

    private void ShowPage(UIPage page)
    {
        if (currentPage == page) return;
        
        currentPage?.Hide();
        currentPage = page;
        currentPage.Show();
    }
    
    private void HidePage()
    {
        currentPage?.Hide();
        currentPage = null;
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
        ShowPage(partySetupPanel);
    }

    public void OffPartySetupPanel()
    {
        HidePage();
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

    public void RefreshGoldPanel(long amount)
    {
        long gold = CurrencyManager.Instance.Gold;
        UIEffect.CounterTo(goldText, gold - amount, gold, 0.7f);
    }

    public void RefreshExpPanel(long amount)
    {
        long exp = CurrencyManager.Instance.Exp;
        UIEffect.CounterTo(expText, exp - amount, exp, 0.7f);
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
    
    public RectTransform GetPanelIcon(RewardType type)
    {
        switch (type)
        {
            case RewardType.Gold:
                return GoldPanelIcon;
            case RewardType.Exp:
                return ExpPanelIcon;
            default:
                return null;
        }
    }

    #endregion

    #region TrainingPanel

    public void OpenTrainingPanel()
    {
        ShowPage(trainingPanel);
    }

    public void OffTrainingPanel()
    {
        HidePage();
    }

    #endregion

    #region MainQuestPanel

    public void OpenMainQuestPanel()
    {
        mainQuestPanel.Show();
    }

    public void OffMainQuestPanel()
    {
        mainQuestPanel.Hide();
    }

    #endregion
    
}
