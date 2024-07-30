using UnityEngine;
using UnityEngine.UI;

public class PlayerRespawnn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;
    [SerializeField] private int maxLives = 3; // Maximum lives (changed from maxDeaths)
    [SerializeField] private Image[] heartImages; // Array of heart images in the UI
    private int currentLives; // Current lives (changed from currentDeaths)
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
        currentLives = maxLives;
        UpdateHeartUI();
    }

    private void OnPlayerDeath()
    {
        currentLives--;
        UpdateHeartUI();

        if (currentLives <= 0)
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
        if (checkpointSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(checkpointSound);
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        uiManager1.GameOver();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CheckPointt"))
        {
            currentCheckpoint = collision.transform;
            collision.GetComponent<Collider2D>().enabled = false;
            if (checkpointSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(checkpointSound);
            }
        }
    }

    private void UpdateHeartUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < currentLives)
            {
                heartImages[i].enabled = true;
            }
            else
            {
                heartImages[i].enabled = false;
            }
        }
    }
}