using System.Collections;
using System.Collections.Generic;
// using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    private float horizontal;
    public float speed = 8f;
    public float jumpPower = 16f;
    private int hearts = 1;

    [SerializeField] Rigidbody rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.localScale = new Vector3(0.5f, 1f, 1f);
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpPower, 0);
        }
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f, 0);
        }
        if (hearts == 0)
        {
            EndGame();
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(horizontal * speed, rb.linearVelocity.y, 0);

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
            GameManager.instance.IncreaseScore(10);
        }

        if (collider.gameObject.CompareTag("Enemy"))
        {
            Vector3 contactPoint = collider.ClosestPoint(transform.position);
            float playerBottom = transform.position.y - (GetComponent<Collider>()?.bounds.extents.y ?? 0f);
            float enemyTop = collider.bounds.max.y;

            if (playerBottom > enemyTop - 0.3f)
            {
                print("Enemy defeated!");
                Destroy(collider.gameObject);
            }
            else
            {
                print("Player hit by enemy!");
                LoseHeart();
            }
        }

        if (collider.gameObject.CompareTag("fireball"))
        {
            print("Player hit by Enemy!");
            LoseHeart();
        }

        if (collider.gameObject.CompareTag("PowerUp"))
        {
            print("Power Up Acquired!");
            PlayerPowerUp();
            Destroy(collider.gameObject);
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

    private void PlayerPowerUp()
    {
        hearts++;
        transform.localScale = new Vector3(1f, 1.5f, 1f);
    }

    private void LoseHeart()
    {
        hearts--;
        transform.localScale = new Vector3(0.5f, 1f, 1f);
        if (hearts == 0)
        {
            EndGame();
        }
    }
}