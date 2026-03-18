using UnityEngine;

public class DamageTextSpawner
{
    private RectTransform damageTextRect => UIManager.Instance.DamageTextRect;
    
    public void Spawn(Vector2 unitPos, float damage)
    {
        GameObject floatingTextObj = PoolManager.Instance.Get(ObjType.FloatingText);
        floatingTextObj.transform.parent = damageTextRect;

        float randPosX = Random.Range(-0.2f, 0.2f);
        float randPosY = Random.Range(0f, 0.2f);
        Vector2 spawnPos = unitPos + new Vector2(randPosX, randPosY);
        floatingTextObj.transform.position = spawnPos;
        
        floatingTextObj.TryGetComponent<FloatingText>(out var floatingText);
        floatingText.Init(damage + "", 0.7f);
        floatingText.gameObject.SetActive(true);
    }
}
