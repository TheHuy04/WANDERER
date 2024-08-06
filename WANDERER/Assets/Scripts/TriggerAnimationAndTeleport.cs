using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerAnimationAndTeleport : MonoBehaviour
{
    public string animationTriggerName = "Teleport"; // Name of the trigger parameter in your Animator
    public string nextSceneName = "_MainMenu"; // Name of the scene to load
    private Animator animator;
    [SerializeField] private AudioClip teleSound;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("No Animator component found on this GameObject.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Assuming the player has the tag "Player"
        {
            StartCoroutine(TriggerAnimationAndTeleportCoroutine());
        }
    }

    IEnumerator TriggerAnimationAndTeleportCoroutine()
    {
        if (SoundManager.instance != null)
        {
            animator.SetTrigger(animationTriggerName); // Trigger the animation
            yield return new WaitForSeconds(4f); // Wait for 2 seconds
            SoundManager.instance.PlaySound(teleSound);
            yield return new WaitForSeconds(0.75f);
            SceneManager.LoadScene(nextSceneName); // Load the next scene
        }
    }
}
