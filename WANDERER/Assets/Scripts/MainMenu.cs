using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public string nextSceneName = "SnowMountain";
    public Slider soundSlider;
    public Slider musicSlider;

    private void Start()
    {
        // Initialize slider values
        soundSlider.value = SoundManager.instance.GetSoundVolume();
        musicSlider.value = SoundManager.instance.GetMusicVolume();

        // Add listeners to sliders
        soundSlider.onValueChanged.AddListener(SetSoundVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    public void Play()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    public void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void SetSoundVolume(float volume)
    {
        SoundManager.instance.SetSoundVolume(volume);
    }

    public void SetMusicVolume(float volume)
    {
        SoundManager.instance.SetMusicVolume(volume);
    }
}