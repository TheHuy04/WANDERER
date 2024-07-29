using UnityEngine;

public class PlayerRespawnn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;
    [SerializeField] private int maxDeaths = 3; // Maximum allowed deaths before game over
    private int currentDeaths = 0;
    private Transform currentCheckpoint;
    private DamageAble damageAble;
    private AudioSource audioSource;

    private void Awake()
    {
        damageAble = GetComponent<DamageAble>();
        audioSource = GetComponent<AudioSource>();
        damageAble.damageableDeath.AddListener(OnPlayerDeath);
    }

    private void OnPlayerDeath()
    {
        currentDeaths++;
        if (currentDeaths >= maxDeaths)
        {
            GameOver();
        }
        else
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        if (currentCheckpoint != null)
        {
            transform.position = currentCheckpoint.position;
            damageAble.Health = damageAble.MaxHealth; // Reset health on respawn
            damageAble.IsAlive = true; // Ensure player is alive

            // Play checkpoint sound if available
            if (checkpointSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(checkpointSound);
            }
        }
        else
        {
            Debug.LogWarning("No checkpoint set!");
        }
    }

    private void GameOver()
    {
        // Game Over logic here
        Debug.Log("Game Over");
        // You can add more logic to handle the game over state, such as showing a game over screen
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CheckPointt")) // Ensure this matches your checkpoint tag
        {
            currentCheckpoint = collision.transform;
            collision.GetComponent<Collider2D>().enabled = false;

            // Optionally play checkpoint sound
            if (checkpointSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(checkpointSound);
            }
        }
    }
}
