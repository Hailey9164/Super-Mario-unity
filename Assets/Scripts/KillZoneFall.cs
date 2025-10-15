using UnityEngine;

public class KillZoneFall : MonoBehaviour
{
    [Tooltip("If the player falls below this Y, trigger death.")]
    public float killY = -100f;

    Transform player;
    PlayerHealth playerHealth;

    void Awake()
    {
        var playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
        {
            player = playerGO.transform;
            playerHealth = playerGO.GetComponent<PlayerHealth>();
        }
        else
        {
            Debug.LogError("KillZoneFall: No GameObject with tag 'Player' found.");
        }
    }

    void Update()
    {
        if (!player) return;

        if (player.position.y < killY)
        {
            Debug.Log("KillZoneFall: player fell below threshold -> Die()");
            playerHealth?.Die();  // <-- use Die(), not Kill()
        }
    }
}
