using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 200f; // New variable for rotation speed
    public float destroyDistance = 0.1f;
    private Transform player;
    private Rigidbody2D rb; // New variable for Rigidbody2D

    private Golem golem; // Reference to the Golem

    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }

    public void SetGolem(Golem golem)
    {
        this.golem = golem;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (player != null)
        {
            // Calculate the direction to the player
            Vector2 direction = (player.position - transform.position).normalized;

            // Calculate the angle to rotate towards
            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            // Apply rotation to the projectile
            rb.angularVelocity = -rotateAmount * rotationSpeed;

            // Move the projectile forward
            rb.velocity = transform.up * speed;

            // Check if the projectile is close enough to the player to destroy it
            if (Vector2.Distance(transform.position, player.position) < destroyDistance)
            {
                DestroyProjectile();
            }
        }
        else
        {
            // If player is not found, destroy the projectile
            DestroyProjectile();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the projectile hits the player or the ground
        if (other.CompareTag("Player") || other.CompareTag("Ground"))
        {
            DestroyProjectile();
        }
    }

    private void DestroyProjectile()
    {
        if (golem != null)
        {
            golem.OnProjectileDestroyed(gameObject);
        }
        Destroy(gameObject);
    }
}
