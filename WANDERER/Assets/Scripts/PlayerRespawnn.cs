using UnityEngine;

public class PlayerRespawnn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;
    [SerializeField] private int maxDeaths = 2; // Maximum allowed deaths before game over
    private int currentDeaths = 0;
    private Transform currentCheckpoint;
    private DamageAble damageAble;
    private AudioSource audioSource;
    private UIManager1 uiManager1;

    private void Awake()
    {
        damageAble = GetComponent<DamageAble>();
        audioSource = GetComponent<AudioSource>();
        damageAble.damageableDeath.AddListener(OnPlayerDeath);
        uiManager1 = FindObjectOfType<UIManager1>();
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
            if (currentCheckpoint != null)
            {
                Respawn();
            }
            else
            {
                GameOver();
            }
        }
    }

    public void Respawn()
    {
        transform.position = currentCheckpoint.position;
        damageAble.Respawn();

        // Play checkpoint sound if available
        if (checkpointSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(checkpointSound);
        }
    }

    public void GameOver()
    {
        // Game Over logic here
        Debug.Log("Game Over");
        // You can add more logic to handle the game over state, such as showing a game over screen
        uiManager1.GameOver();
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
