using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;

    // Call this method to update the health bar
    public void SetHealth(int currentHealth, int maxHealth)
    {
        Debug.Log("Setting health: " + currentHealth + "/" + maxHealth);
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    // Call this method to show the health bar
    public void Show()
    {
        Debug.Log("Showing health bar");
        gameObject.SetActive(true);
    }

    // Call this method to hide the health bar
    public void Hide()
    {
        Debug.Log("Hiding health bar");
        gameObject.SetActive(false);
    }

    private void Start()
    {
        // Ensure the health bar is hidden by default
        Hide();
    }
}
