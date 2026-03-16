using UnityEngine;
using UnityEngine.UI;

public class UnitListSlotUI : MonoBehaviour
{
    [SerializeField] private Image unitImage;
    [SerializeField] private Text unitNameText;

    public void SetSlot(Sprite unitSptrite, string unitName)
    {
        unitImage.sprite = unitSptrite;
        unitNameText.text = unitName;
    }
}
