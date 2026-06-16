using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarGradeUI : MonoBehaviour
{
    [Header("=== Resource ===")]
    [SerializeField] protected GameObject starPrefab;
    private readonly List<Image> stars = new();
    private Sprite starSprite;

    public void SetStars(int grade, Sprite starSpr)
    {
        for (int i = stars.Count; i < grade; i++)
        {
            GameObject star = Instantiate(starPrefab, transform);
            stars.Add(star.GetComponent<Image>());
        }

        starSprite = starSpr;

        for (int i = 0; i < stars.Count; i++)
        {
            bool active = i < grade;
            stars[i].sprite = starSprite;
            stars[i].gameObject.SetActive(active);
        }
    }
    
    public void SetStars(int grade)
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
