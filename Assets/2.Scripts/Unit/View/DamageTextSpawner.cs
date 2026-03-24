using UnityEngine;

public class DamageTextSpawner
{
    public void Spawn(Vector2 unitPos, float damage)
    {
        FloatingText floatingText = PoolManager.Instance.Get<FloatingText>();

        if (floatingText == null)
        {
            Debug.LogError("Spawn Floating Text Fail.");
            return;
        }
        
        float randPosX = Random.Range(-0.2f, 0.2f);
        float randPosY = Random.Range(0f, 0.2f);
        Vector2 spawnPos = unitPos + new Vector2(randPosX, randPosY);
        floatingText.transform.position = spawnPos;
        
        floatingText.Init(damage + "", 0.7f);
        floatingText.gameObject.SetActive(true);
    }
}
