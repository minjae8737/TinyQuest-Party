using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaCardUI : ClickCardUI
{
    [Header("=== Reference ===")]
    [SerializeField] protected Image unitImage;
    [SerializeField] protected TMP_Text unitGardeText;
    [SerializeField] protected TMP_Text unitNameText;
    [SerializeField] protected StarGradeUI StarGradeUI;
    
    public void SetCard(GachaResultData resultData, TMP_ColorGradient gradeGradient)
    {
        unitImage.sprite = resultData.Icon;

        unitGardeText.text = resultData.UnitGradeType.ToString();  
        unitGardeText.colorGradientPreset = gradeGradient;
        unitNameText.text = resultData.UnitName.ToString();

        // StarGradeUI.SetStars(dto.StarGrade, starSprite);
    }
    
    
}
