using UnityEngine;
using UnityEngine.UI;

public class HealthBarMonsters : MonoBehaviour
{
    public Slider slider;
    public float displayDuration = 3f;
    public float fadeOutDuration = 1f;

    private float displayTimer;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        Hide();
    }

    public void SetHealth(int currentHealth, int maxHealth)
    {
        slider.value = (float)currentHealth / maxHealth;
        Show();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        canvasGroup.alpha = 1f;
        displayTimer = displayDuration;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        canvasGroup.alpha = 0f;
    }

    private void Update()
    {
        if (displayTimer > 0)
        {
            displayTimer -= Time.deltaTime;
            if (displayTimer <= 0)
            {
                StartFadeOut();
            }
        }

        // Always face the camera
        if (Camera.main != null)
        {
            transform.forward = Camera.main.transform.forward;
        }
    }

    private void StartFadeOut()
    {
        LeanTween.value(gameObject, UpdateAlpha, canvasGroup.alpha, 0f, fadeOutDuration)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(() => gameObject.SetActive(false));
    }

    private void UpdateAlpha(float value)
    {
        canvasGroup.alpha = value;
    }
}