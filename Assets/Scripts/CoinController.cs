using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider))]
public class CoinController : MonoBehaviour
{
    [Header("Spin Settings")]
    public float rotationSpeed = 100f;

    [Header("Pop Physics Settings (for block-spawned coins only)")]
    [Tooltip("Upward velocity applied when coin is spawned from a block.")]
    public float popVelocity = 6f;

    [Tooltip("How far above/below the return height we allow before snapping.")]
    public float settleEpsilon = 0.02f;

    [Tooltip("Safety timeout in seconds to avoid infinite waits.")]
    public float popTimeout = 2.0f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Coins placed in the world shouldn’t fall
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.freezeRotation = false;
        }
    }

    void Start()
    {
        // Your original visual tweak
        transform.Rotate(0f, 0f, 90f, Space.Self);
    }

    void Update()
    {
        float angle = rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, angle, Space.World);
    }

    /// <summary>
    /// Called ONLY by BlockBase after emerging.
    /// Enables physics, pops up, then waits until the coin falls back to returnY.
    /// Finally re-disables physics so floating coins elsewhere remain unaffected.
    /// </summary>
    public void Pop(float returnY)
    {
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.freezeRotation = true;
        }

        StartCoroutine(PopRoutine(returnY));
    }

    // Overload for safety; if called without a returnY, use current height
    public void Pop()
    {
        Pop(transform.position.y);
    }

    private IEnumerator PopRoutine(float returnY)
    {
        // Turn physics ON and launch upward
        rb.isKinematic = false;
        rb.useGravity = true;
#if UNITY_6000_0_OR_NEWER
        rb.linearVelocity = new Vector3(0f, popVelocity, 0f);
#else
        rb.velocity = new Vector3(0f, popVelocity, 0f);
#endif

        float startTime = Time.time;

        while (true)
        {
            if (Time.time - startTime > popTimeout) break;

#if UNITY_6000_0_OR_NEWER
            if (rb.linearVelocity.y <= 0f) break;
#else
            if (rb.velocity.y <= 0f) break;
#endif
            yield return null;
        }

        // 2) Now wait until it falls back to (or below) the return height
        while (true)
        {
            if (Time.time - startTime > popTimeout) break;

            if (transform.position.y <= returnY + settleEpsilon) break;

            yield return null;
        }

        // 3) Snap to return height and turn physics OFF again
        var p = transform.position;
        p.y = returnY;
        transform.position = p;

#if UNITY_6000_0_OR_NEWER
        rb.linearVelocity = Vector3.zero;
#else
        rb.velocity = Vector3.zero;
#endif
        rb.useGravity = false;
        rb.isKinematic = true;
    }
}
