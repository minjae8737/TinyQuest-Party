using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaPanel : UIPage
{
    [Header("=== Summon Panel ===")] 
    [SerializeField] private GameObject summonPanel;
    [SerializeField] private Button summonBtn1;
    [SerializeField] private Button summonBtn10;
    
    [Header("=== Result Panel ===")]
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private Button exitBtn;
    [SerializeField] private Button summonBtn1_result;
    [SerializeField] private Button summonBtn10_result;

    [SerializeField] private List<GachaCardUI> cards;
    [SerializeField] private List<GachaCardUI> leftCardGroup;
    [SerializeField] private List<GachaCardUI> centerCardGroup;
    [SerializeField] private List<GachaCardUI> rightCardGroup;

    [SerializeField] private TMP_ColorGradient[] unitGradeGradients;
    
    // Caching
    private Vector2 panelOriginPos;
    
    public void Init()
    {
        // Summon Panel
        summonBtn1.onClick.AddListener(() => OnclickSummonBtn(1));
        summonBtn1.onClick.AddListener(() => UIEffect.Punch(summonBtn1.transform as RectTransform));
        summonBtn10.onClick.AddListener(() => OnclickSummonBtn(10));
        summonBtn10.onClick.AddListener(() => UIEffect.Punch(summonBtn10.transform as RectTransform));
        
        // Result Panel
        exitBtn.onClick.AddListener(HideGachaResultPanel);
        exitBtn.onClick.AddListener(() => UIEffect.Punch(exitBtn.transform as RectTransform));
        summonBtn1_result.onClick.AddListener(() => OnclickSummonBtn(1));
        summonBtn1_result.onClick.AddListener(() => UIEffect.Punch(summonBtn1_result.transform as RectTransform));
        summonBtn10_result.onClick.AddListener(() => OnclickSummonBtn(10));
        summonBtn10_result.onClick.AddListener(() => UIEffect.Punch(summonBtn10_result.transform as RectTransform));

        // Caching
        panelOriginPos = (summonPanel.transform as RectTransform).anchoredPosition;
    }

    public override void Show()
    {
        gameObject.SetActive(true);
        AudioManager.Instance.PlaySfx(Sfx.UIOpen);
        UIEffect.SlideUp(summonPanel.transform as RectTransform, panelOriginPos, 50f, 0.7f);    
    }
    
    public override void Hide()
    { 
        gameObject.SetActive(false);
        AudioManager.Instance.PlaySfx(Sfx.UIClose);    
    }

    #region Summon Panel

    private async void OnclickSummonBtn(int count)
    {
        List<GachaResultData> gachaResultDatas = await GachaManager.Instance.DoGacha(count);

        RefreshResultPanel(gachaResultDatas);
        ShowGachaResultPanel();
    }

    #endregion

    #region Result Panel

    private void RefreshResultPanel(List<GachaResultData> gachaResultDatas)
    {
        bool isOnce = gachaResultDatas.Count == 1;
        
        summonBtn1_result.gameObject.SetActive(isOnce);
        summonBtn10_result.gameObject.SetActive(!isOnce);
        
        if (cards.Count != 10)
        {
            Debug.LogError("");
            return;
        }
        
        cards[0].gameObject.SetActive(true);
        cards[1].gameObject.SetActive(!isOnce);
        cards[2].gameObject.SetActive(!isOnce);
        cards[3].gameObject.SetActive(!isOnce);
        cards[4].gameObject.SetActive(!isOnce);
        cards[5].gameObject.SetActive(!isOnce);
        cards[6].gameObject.SetActive(!isOnce);
        cards[7].gameObject.SetActive(!isOnce);
        cards[8].gameObject.SetActive(!isOnce);
        cards[9].gameObject.SetActive(!isOnce);

        cards[0].SetCard(gachaResultDatas[0], unitGradeGradients[(int)gachaResultDatas[0].UnitGradeType]);

        if (!isOnce)
        {
            for (int i = 1; i < gachaResultDatas.Count; i++)
            {
                cards[i].SetCard(gachaResultDatas[i], unitGradeGradients[(int)gachaResultDatas[i].UnitGradeType]);
            }
        }
    }

    private void ShowGachaResultPanel()
    {
        resultPanel.SetActive(true);
    }
    
    private void HideGachaResultPanel()
    {
        resultPanel.SetActive(false);
    }
    
    #endregion
}
