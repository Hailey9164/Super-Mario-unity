using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerEnemyCollision : MonoBehaviour
{
    [Header("Stomp settings")]
    public float stompBounce = 8f;     // up impulse when you stomp an enemy
    public float topAllowance = 0.25f; // how “forgiving” the top hit is

    Rigidbody rb;
    Collider playerCol;
    PlayerHealth health;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerCol = GetComponent<Collider>();
        health = GetComponent<PlayerHealth>();
    }

    // Works for normal colliders (IsTrigger = false)
    void OnCollisionEnter(Collision collision)
    {
        HandleEnemyContact(collision.collider);
    }

    // Works if enemies use trigger colliders (IsTrigger = true)
    void OnTriggerEnter(Collider other)
    {
        HandleEnemyContact(other);
    }

    void HandleEnemyContact(Collider enemyCol)
    {
        if (!enemyCol || !enemyCol.CompareTag("Enemy")) return;

        // Where is the bottom of the player and the top of the enemy?
        float playerBottom = playerCol.bounds.min.y;
        float enemyTop = enemyCol.bounds.max.y;

        bool isFallingOrDownward = rb.linearVelocity.y <= 0.01f;
        bool stomp = isFallingOrDownward && playerBottom > (enemyTop - topAllowance);

        Debug.Log($"[EnemyContact] stomp={stomp} vY={rb.linearVelocity.y:0.00} playerBottom={playerBottom:0.00} enemyTop={enemyTop:0.00}");

        if (stomp)
        {
            // Bounce up
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, stompBounce, rb.linearVelocity.z);

            // Try kill the enemy via its own script; otherwise just destroy the root
            var killable = enemyCol.GetComponentInParent<MonoBehaviour>();
            var enemyGo = (killable ? killable.gameObject : enemyCol.transform.root.gameObject);
            Destroy(enemyGo);
        }
        else
        {
            if (health) health.Die();
            else Debug.LogWarning("PlayerEnemyCollision: PlayerHealth missing on Player.");
        }
    }
}
