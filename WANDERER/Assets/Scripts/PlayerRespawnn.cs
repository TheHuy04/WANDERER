using UnityEngine;
using UnityEngine.UI;

public class PlayerRespawnn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;
    [SerializeField] private AudioClip chestSound;
    [SerializeField] private AudioClip openChestSound;
    [SerializeField] private int maxLives = 3;
    [SerializeField] private Image[] heartImages;
    [SerializeField] private int points = 0;
    [SerializeField] private int pointsPerTreasure = 10;
    [SerializeField] private float interactionDistance = 2f;
    [SerializeField] private KeyCode interactionKey = KeyCode.E;

    private Vector3 startingPosition;
    private int currentLives;
    private Transform currentCheckpoint;
    private DamageAble damageAble;
    private AudioSource audioSource;
    private UIManager1 uiManager1;
    Chest Chest;

    private void Awake()
    {
        damageAble = GetComponent<DamageAble>();
        audioSource = GetComponent<AudioSource>();
        damageAble.damageableDeath.AddListener(OnPlayerDeath);
        uiManager1 = FindObjectOfType<UIManager1>();
        currentLives = maxLives;
        UpdateHeartUI();
        startingPosition = transform.position;
        UpdatePointsUI();
        Chest = GetComponent<Chest>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(interactionKey))
        {
            Debug.Log("Interaction key pressed");
            TryInteractWithChest();
        }
    }

    private void TryInteractWithChest()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactionDistance);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Chest"))
            {
                Chest chest = collider.GetComponent<Chest>();
                if (chest != null)
                {
                    if (!chest.IsOpen)
                    {
                        CollectTreasure(chest.gameObject);
                        return;
                    }
                }
                else
                {
                    Debug.Log("Chest component not found on object");
                }
            }
        }
    }

    private void CollectTreasure(GameObject treasure)
    {
        points += pointsPerTreasure;
        UpdatePointsUI();

        // Play sound effect for collecting treasure
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySound(chestSound);
        }

        // If it's a chest, open it
        Chest chest = treasure.GetComponent<Chest>();
        if (chest != null)
        {
            chest.Open();
            SoundManager.instance.PlaySound(openChestSound);
        }
        else
        {
            // If it's not a chest, disable its collider
            treasure.GetComponent<Collider2D>().enabled = false;
        }
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
                ResetPlayerWithoutCheckpoint();
            }
        }
    }

    private void ResetPlayerWithoutCheckpoint()
    {
        transform.position = startingPosition;
        damageAble.Respawn();
    }

    public void Respawn()
    {
        transform.position = currentCheckpoint.position;
        damageAble.Respawn();
        if (checkpointSound != null && SoundManager.instance != null)
        {
            SoundManager.instance.PlaySound(checkpointSound);
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
            if (checkpointSound != null && SoundManager.instance != null)
            {
                SoundManager.instance.PlaySound(checkpointSound);
            }
        }
        else if (collision.CompareTag("Treasure") && !collision.CompareTag("Chest"))
        {
            CollectTreasure(collision.gameObject);
        }
    }

    private void UpdateHeartUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].enabled = i < currentLives;
        }
    }

    public bool AddLife()
    {
        if (currentLives < maxLives)
        {
            currentLives++;
            UpdateHeartUI();
            return true;
        }
        return false;
    }

    private void UpdatePointsUI()
    {
        if (uiManager1 != null)
        {
            uiManager1.UpdatePointsDisplay(points);
        }
    }
}