using UnityEngine;

public class LifePickup : MonoBehaviour
{
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioClip maxLivesSound; // New sound for when at max lives
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Make sure your player has the "Player" tag
        {
            PlayerRespawnn playerRespawn = collision.GetComponent<PlayerRespawnn>();
            if (playerRespawn != null)
            {
                bool lifeAdded = playerRespawn.AddLife();

                if (lifeAdded)
                {
                    // Play pickup sound if assigned
                    if (pickupSound != null)
                    {
                        audioSource.PlayOneShot(pickupSound);
                    }

                    // Disable the sprite renderer and collider
                    GetComponent<SpriteRenderer>().enabled = false;
                    GetComponent<Collider2D>().enabled = false;

                    // Destroy the game object after playing the sound (if any)
                    float destroyDelay = pickupSound != null ? pickupSound.length : 0f;
                    Destroy(gameObject, destroyDelay);
                }
                else
                {
                    // Player is at max lives
                    if (maxLivesSound != null)
                    {
                        audioSource.PlayOneShot(maxLivesSound);
                    }
                    // Optionally, you could add a visual feedback here, like a floating text saying "Max Lives!"
                }
            }
        }
    }
}