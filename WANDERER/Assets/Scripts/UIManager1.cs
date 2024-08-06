using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager1 : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public GameObject healthTextPrefab;
    [Header("Game Over")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;
    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen;
    [Header("Points")]
    [SerializeField] private TextMeshProUGUI pointsText;
    private DamageAble damageAble;
    public Canvas gameCanvas;

    private void Awake()
    {
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
        gameCanvas = FindObjectOfType<Canvas>();
        damageAble = GetComponent<DamageAble>();
        UpdatePointsDisplay(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame(!pauseScreen.activeInHierarchy);
        }
    }

    private void OnEnable()
    {
        CharacterEvents.characterDamaged += CharacterTookDamage;
        CharacterEvents.characterHealed += CharacterHealed;
    }

    private void OnDisable()
    {
        CharacterEvents.characterDamaged -= CharacterTookDamage;
        CharacterEvents.characterHealed -= CharacterHealed;
    }

    public void CharacterTookDamage(GameObject character, int damageReceived)
    {
        SpawnFloatingText(character, damageTextPrefab, damageReceived.ToString());
    }

    public void CharacterHealed(GameObject character, int healthRestored)
    {
        SpawnFloatingText(character, healthTextPrefab, healthRestored.ToString());
    }

    private void SpawnFloatingText(GameObject character, GameObject textPrefab, string text)
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
        TMP_Text tmpText = Instantiate(textPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();
        tmpText.text = text;
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        SoundManager.instance.PlaySound(gameOverSound);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void SoundVolume()
    {
        float currentVolume = SoundManager.instance.GetSoundVolume();
        float newVolume = (currentVolume + 0.2f) % 1.0f;
        SoundManager.instance.SetSoundVolume(newVolume);
    }

    public void MusicVolume()
    {
        float currentVolume = SoundManager.instance.GetMusicVolume();
        float newVolume = (currentVolume + 0.2f) % 1.0f;
        SoundManager.instance.SetMusicVolume(newVolume);
    }

    public void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void PauseGame(bool status)
    {
        pauseScreen.SetActive(status);
        Time.timeScale = status ? 0 : 1;
    }

    public void UpdatePointsDisplay(int points)
    {
        if (pointsText != null)
        {
            pointsText.text = $"Points: {points}";
        }
    }
}