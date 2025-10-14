using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PowerUpEmergeAndMove : MonoBehaviour
{
    public float moveSpeed = 2.5f;
    public bool beginMoving = false;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Ensure it collides with ground / pipes, not trigger
        var col = GetComponent<Collider>();
    }

    // Called by BlockBase AFTER the emerge finishes
    public void Begin()
    {
        beginMoving = true;
    }

    void FixedUpdate()
    {
        if (!beginMoving) return;

        // Constant rightward motion; gravity/physics still apply on Y
        Vector3 v = rb.linearVelocity;
        v.x = moveSpeed;
        rb.linearVelocity = v;
    }
}
