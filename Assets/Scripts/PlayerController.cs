using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move/Jump")]
    public float speed = 8f;
    public float jumpPower = 16f;

    [Header("Ground Check (set in Inspector)")]
    [SerializeField] private Transform groundCheck;   // drag a child here
    [SerializeField] private LayerMask groundLayer;   // tick the "Ground" layer here
    [SerializeField] private float groundRadius = 0.25f;

    private Rigidbody rb;
    private float horizontal;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // 2.5D feel: freeze Z position & all rotations
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Vector3 v = rb.linearVelocity;
            v.y = jumpPower;
            rb.linearVelocity = v;
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f, rb.linearVelocity.z);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(horizontal * speed, rb.linearVelocity.y, rb.linearVelocity.z);
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundRadius, groundLayer, QueryTriggerInteraction.Ignore);
    }

    // visualize the ground sphere (helps you place it)
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}
