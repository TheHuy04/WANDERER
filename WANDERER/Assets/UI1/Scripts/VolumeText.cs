using UnityEngine;
using TMPro;

public class VolumeText : MonoBehaviour
{
    [SerializeField] private string volumeName;
    [SerializeField] private string textIntro; // Prefix like "Sound: " or "Music: "
    private TextMeshProUGUI txt;

    private void Awake()
    {
        txt = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        UpdateVolume();
    }

    private void UpdateVolume()
    {
        float volumeValue = PlayerPrefs.GetFloat(volumeName, 1f) * 100; // Default to 100% if no value is found
        txt.text = $"{textIntro} {volumeValue:F0}"; // F0 formats the float to a whole number
    }
}
