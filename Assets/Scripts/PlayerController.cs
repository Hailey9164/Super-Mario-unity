using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    private float horizontal;
    public float speed = 8f;
    public float jumpPower = 16f;

    [SerializeField] Rigidbody rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpPower, 0);
        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.5f, 0);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(horizontal * speed, rb.velocity.y, 0);

    }

    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Coin"))
        {
            print("Coin collected!");
            Destroy(collider.gameObject);
        }

        if (collider.gameObject.CompareTag("Enemy"))
        {
            // Check if the collision point is on top of the enemy
            // Use the player's position and the enemy's bounds to determine if the player is above
            Vector3 contactPoint = collider.ClosestPoint(transform.position);
            float playerBottom = transform.position.y - (GetComponent<Collider>()?.bounds.extents.y ?? 0f);
            float enemyTop = collider.bounds.max.y;

            if (playerBottom > enemyTop - 0.3f) // Allow a small margin
            {
                print("Enemy defeated!");
                Destroy(collider.gameObject);
            }
            else
            {
                print("Player hit by enemy!");
                EndGame();
            }
        }

        if (collider.gameObject.CompareTag("fireball"))
        {
            print("Player hit by Enemy!");
            EndGame();
        }
    }
    private void EndGame()
    {
        print("Game Over!");
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}