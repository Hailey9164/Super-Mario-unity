using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class MushroomPowerUp3D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;          // try 3–4
    public bool autoStart = true;          // start walking automatically
    public float popDelay = 0.12f;         // delay after popping from a block

    Rigidbody rb;
    int dir = 1;                           // -1 = left, +1 = right
    bool active;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.linearDamping = 0f;
        rb.angularDamping = 0.05f;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        // IMPORTANT: freeze rotations so it doesn’t tip over
        rb.constraints = RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationY |
                         RigidbodyConstraints.FreezeRotationZ;
    }

    void Start()
    {
        if (autoStart)
        {
            dir = Random.value < 0.5f ? -1 : 1;
            active = true;                 // walk immediately so you can verify
        }
    }

    void FixedUpdate()
    {
        if (!active) return;

        // constant horizontal push
        var v = rb.linearVelocity;
        v.x = dir * moveSpeed;
        v.z = 0f;
        rb.linearVelocity = v;
    }

    // Flip when we hit something from the side (a wall/box/pipe, etc.)
    void OnCollisionEnter(Collision c)
    {
        foreach (var cp in c.contacts)
        {
            Vector3 n = cp.normal; // surface normal of what we hit
            if (Mathf.Abs(n.x) > 0.5f)        // mostly horizontal contact → side hit
            {
                dir = n.x > 0 ? 1 : -1;       // move away from the surface
                break;
            }
        }
    }

    // Called by the block when spawned, or you can call manually
    public void PopFromBlock(Vector3 up)
    {
        rb.AddForce(up.normalized * 4f, ForceMode.VelocityChange);
        dir = Random.value < 0.5f ? -1 : 1;
        Invoke(nameof(Activate), popDelay);
    }

    void Activate() => active = true;

    // Called by PowerUpPickup on the child trigger
    public void OnPickedUp()
    {
        // TODO: apply growth to player here if desired
        Destroy(gameObject);
    }
}
