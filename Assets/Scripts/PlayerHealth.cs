using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private bool isDead = false;

    [Header("References")]
    public DeathManager deathManager;

    void Start()
    {
        // Unity 2022+ replacement for FindObjectOfType
        if (deathManager == null)
            deathManager = Object.FindFirstObjectByType<DeathManager>();
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        var rb = GetComponent<Rigidbody>();
        if (rb)
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        var controller = GetComponent<PlayerController>();
        if (controller) controller.enabled = false;

        if (deathManager)
            deathManager.PlayerDied();
        else
            Debug.LogWarning("No DeathManager found in scene.");
    }
}
