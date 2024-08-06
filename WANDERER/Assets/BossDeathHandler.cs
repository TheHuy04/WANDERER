using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

public class BossDeathHandler : MonoBehaviour
{
    public DamageAble damageable; // Reference to the DamageAble script
    [SerializeField] private float deathDelay = 2.0f; // Delay before transitioning to credits scene
    [SerializeField] private string creditsSceneName = "CreditsScene"; // Name of the credits scene

    private void Start()
    {
        if (damageable != null)
        {
            damageable.damageableDeath.AddListener(OnBossDeath);
        }
        else
        {
            Debug.LogWarning("DamageAble reference not set on BossDeathHandler script!");
        }
    }

    private void OnBossDeath()
    {
        StartCoroutine(TransitionToCredits());
    }

    private IEnumerator TransitionToCredits()
    {
        yield return new WaitForSeconds(deathDelay);
        SceneManager.LoadScene(creditsSceneName);
    }
}
