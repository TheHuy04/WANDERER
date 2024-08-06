using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private AudioSource soundSource;
    private AudioSource musicSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        soundSource = GetComponent<AudioSource>();
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();

        // Initialize volumes
        SetSoundVolume(GetSoundVolume());
        SetMusicVolume(GetMusicVolume());
    }

    public void PlaySound(AudioClip _sound)
    {
        soundSource.PlayOneShot(_sound);
    }

    public void ChangeSoundVolume(float _change)
    {
        SetSoundVolume(GetSoundVolume() + _change);
    }

    public void ChangeMusicVolume(float _change)
    {
        SetMusicVolume(GetMusicVolume() + _change);
    }

    public float GetSoundVolume()
    {
        return PlayerPrefs.GetFloat("soundVolume", 1f);
    }

    public float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat("musicVolume", 1f);
    }

    public void SetSoundVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("soundVolume", volume);
        soundSource.volume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("musicVolume", volume);
        musicSource.volume = volume;
    }
}