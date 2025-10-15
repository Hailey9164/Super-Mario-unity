using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PowerUpPickup : MonoBehaviour
{
    [Tooltip("Root power-up controller (on the parent).")]
    public MushroomPowerUp3D root;

    void Reset()
    {
        // Ensure this collider is a trigger
        var c = GetComponent<Collider>();
        c.isTrigger = true;

        if (root == null)
            root = GetComponentInParent<MushroomPowerUp3D>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Apply the effect here if you like (grow, etc.)
        Debug.Log("Power-up collected!");

        if (root != null) root.OnPickedUp();
        else Destroy(transform.root.gameObject);
    }
}
