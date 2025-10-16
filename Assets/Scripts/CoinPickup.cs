using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [Header("Coin Settings")]
    public int value = 1;
    public AudioClip pickupSound;

    private void OnTriggerEnter(Collider other)
    {
        // Make sure only the player can collect
        if (!other.CompareTag("Player")) return;

        // Play a sound when picked up
        if (pickupSound)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, 0.8f);

        // Add coin value to GameManager’s score
        if (GameManager.Instance != null)
        {
            GameManager.Instance.IncreaseScore(value);
        }
        else
        {
            Debug.LogWarning("GameManager not found in scene!");
        }

        // Destroy this coin object
        Destroy(gameObject);
    }
}
