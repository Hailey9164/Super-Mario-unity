using UnityEngine;

[RequireComponent(typeof(Collider))]
public class QuestionBlock3D : MonoBehaviour
{
    public GameObject coinPrefab;
    public float spawnOffsetY = 0.6f;
    public bool used;

    Collider myCol;

    void Awake()
    {
        myCol = GetComponent<Collider>();
        myCol.isTrigger = false;               // must collide
    }

    void OnCollisionEnter(Collision col)
    {
        // TEMP: spawn on ANY collision so we know spawning works
        Debug.Log($"Block collided with {col.collider.name}");
        if (!used) SpawnCoin();
    }

void SpawnCoin()
{
    used = true;
    Vector3 pos = transform.position + Vector3.up * 1.5f;

    if (coinPrefab == null)
    {
        Debug.LogError($"[{name}] Coin Prefab is NULL!");
        return;
    }

    var coin = Instantiate(coinPrefab, pos, Quaternion.identity);
    coin.name = "COIN_DEBUG";
    Debug.Log($"Spawned COIN_DEBUG at {pos}");
}

}
