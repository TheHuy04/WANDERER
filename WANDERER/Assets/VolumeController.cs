using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider soundSlider;
    public Slider musicSlider;

    private void Start()
    {
        // Initialize slider values
        soundSlider.value = SoundManager.instance.GetSoundVolume();
        musicSlider.value = SoundManager.instance.GetMusicVolume();

        // Add listeners to sliders
        soundSlider.onValueChanged.AddListener(OnSoundSliderChanged);
        musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
    }

    private void OnSoundSliderChanged(float value)
    {
        SoundManager.instance.SetSoundVolume(value);
    }

    private void OnMusicSliderChanged(float value)
    {
        SoundManager.instance.SetMusicVolume(value);
    }
}