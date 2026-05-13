using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarGradeUI : MonoBehaviour
{
    [Header("=== Resource ===")]
    [SerializeField] protected GameObject starPrefab;
    private readonly List<Image> stars = new();

    public void SetStars(int grade, Sprite starSprite)
    {
        for (int i = stars.Count; i < grade; i++)
        {
            GameObject star = Instantiate(starPrefab, transform);
            stars.Add(star.GetComponent<Image>());
        }

        for (int i = 0; i < stars.Count; i++)
        {
            bool active = i < grade;
            stars[i].sprite = starSprite;
            stars[i].gameObject.SetActive(active);
        }
    }

}
