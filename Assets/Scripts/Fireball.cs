using UnityEngine;

public class Fireball : MonoBehaviour
{

    [SerializeField] private float timer = 5;
    private float fireballTimer;

    public GameObject enemyFire;
    public Transform spawnPoint;

    public float enemySpeed = 500;
    public float fireballLifetime = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        shoot();
    }

    void shoot()
    {
        fireballTimer -= Time.deltaTime;
        if (fireballTimer > 0) return;

        fireballTimer = timer;

        // Find the player (assumes the player GameObject is tagged "Player")
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return; // No player found

        // Calculate direction from spawnPoint to player
        Vector3 direction = (player.transform.position - spawnPoint.position).normalized;

        GameObject fireball = Instantiate(enemyFire, spawnPoint.position, Quaternion.LookRotation(direction));
        Rigidbody rb = fireball.GetComponent<Rigidbody>();
        rb.AddForce(direction * enemySpeed);
        Destroy(fireball, fireballLifetime);
    }
   
}
