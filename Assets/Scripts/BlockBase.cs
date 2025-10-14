using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider))]
public class BlockBase : MonoBehaviour
{
    [Header("Assign the thing inside this block")]
    [Tooltip("Prefab to spawn: Coin OR Power Up.")]
    public GameObject spawnPrefab;          // Coin OR Power Up

    [Header("Spawn point at the TOP of the block")]
    [Tooltip("If left empty, a child named 'SpawnPoint' will be auto-created on Scene instances.")]
    public Transform spawnPoint;            // If null, we’ll auto-place it
    public float riseHeight = 1.0f;
    public float riseTime = 0.25f;

    [Header("Block bump animation")]
    public float bumpHeight = 0.12f;
    public float bumpTime = 0.08f;

    [Header("Rules")]
    [Tooltip("Once used, the block won't spawn again.")]
    public bool used = false;

    private Collider _blockCol;

#if UNITY_EDITOR
    // Safety: don't edit prefab ASSETS in the Project window
    private static bool IsPrefabAsset(GameObject go)
    {
        return PrefabUtility.IsPartOfPrefabAsset(go);
    }
#endif

    void Reset()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying && IsPrefabAsset(gameObject))
        {
            // Avoid prefab-asset parenting errors
            return;
        }
#endif
        EnsureSpawnPointExists();
    }

    void Awake()
    {
        _blockCol = GetComponent<Collider>();
        if (_blockCol == null)
        {
            _blockCol = gameObject.AddComponent<BoxCollider>();
        }
        if (_blockCol.isTrigger) _blockCol.isTrigger = false;

        riseTime = Mathf.Max(0.0001f, riseTime);
        bumpTime = Mathf.Max(0.0001f, bumpTime);

#if UNITY_EDITOR
        if (Application.isPlaying || !IsPrefabAsset(gameObject))
#endif
        {
            if (spawnPoint == null) EnsureSpawnPointExists();
        }
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        riseHeight = Mathf.Max(0f, riseHeight);
        bumpHeight = Mathf.Max(0f, bumpHeight);
        riseTime = Mathf.Max(0.0001f, riseTime);
        bumpTime = Mathf.Max(0.0001f, bumpTime);

        if (_blockCol == null) _blockCol = GetComponent<Collider>();
    }
#endif

    private void EnsureSpawnPointExists()
    {
        if (spawnPoint != null) return;

        var t = new GameObject("SpawnPoint").transform;
        t.SetParent(transform, false);
        t.localPosition = Vector3.up * 0.55f;
        spawnPoint = t;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (used) return;

        var other = collision.collider;
        if (!other || !other.CompareTag("Player")) return;

        // Simple rule: player hit -> trigger
        StartCoroutine(HandleHit());
    }

    IEnumerator HandleHit()
    {
        if (used) yield break;
        used = true;

        // 1) Bump anim
        yield return StartCoroutine(Bump());

        // 2) Spawn item
        if (spawnPrefab == null || spawnPoint == null) yield break;

        var item = Instantiate(spawnPrefab, spawnPoint.position, Quaternion.identity);

        // 3) If it has a Rigidbody, freeze while emerging
        Rigidbody rb = item.GetComponent<Rigidbody>();
        bool hadRB = rb != null;

        if (hadRB)
        {
            rb.isKinematic = true;
#if UNITY_6000_0_OR_NEWER
            rb.linearVelocity = Vector3.zero;
#else
            rb.velocity = Vector3.zero;
#endif
            rb.angularVelocity = Vector3.zero;
        }

        // Emerge upward out of the block
        Vector3 start = item.transform.position;
        Vector3 end = start + Vector3.up * riseHeight;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / riseTime;
            item.transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        if (hadRB)
        {
            rb.isKinematic = false;
        }

        // 4) Kick off behavior
        if (item.TryGetComponent<PowerUpEmergeAndMove>(out var mover))
        {
            mover.Begin();
        }
        else if (item.TryGetComponent<CoinController>(out var coin))
        {
            // Tell the coin to pop and then return to the emerge height
            coin.Pop((end.y + start.y) / 2.0f);
        }
    }

    IEnumerator Bump()
    {
        Vector3 basePos = transform.position;
        Vector3 upPos = basePos + Vector3.up * bumpHeight;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / bumpTime;
            transform.position = Vector3.Lerp(basePos, upPos, t);
            yield return null;
        }

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / bumpTime;
            transform.position = Vector3.Lerp(upPos, basePos, t);
            yield return null;
        }

        transform.position = basePos;
    }
}
