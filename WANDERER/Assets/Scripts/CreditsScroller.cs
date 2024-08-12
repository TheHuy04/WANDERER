using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreditsScroller : MonoBehaviour
{
    public TextMeshProUGUI creditsText; // Reference to the Text component
    public float scrollSpeed = 20f; // Speed at which the credits scroll
    public float startDelay = 2f; // Delay before starting the scroll

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = creditsText.GetComponent<RectTransform>();
        // Start the scrolling after a delay
        Invoke("StartScrolling", startDelay);
    }

    void StartScrolling()
    {
        StartCoroutine(ScrollCredits());
    }

    System.Collections.IEnumerator ScrollCredits()
    {
        // Calculate the height to move
        float totalHeight = rectTransform.rect.height + rectTransform.parent.GetComponent<RectTransform>().rect.height;

        // Move the creditsText upwards
        while (rectTransform.anchoredPosition.y < totalHeight)
        {
            rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
