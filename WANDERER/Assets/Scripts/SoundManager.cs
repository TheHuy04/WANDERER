using System.Collections;
using System.Collections.Generic;
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
        else if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        soundSource = GetComponent<AudioSource>();
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();

        ChangeMusicVolume(0);
        ChangeSoundVolume(0);
    }

    public void PlaySound(AudioClip _sound)
    {
        soundSource.PlayOneShot(_sound);
    }

    public void ChangeSoundVolume(float _change)
    {
        ChangeSourceVolume(1, "soundVolume", _change, soundSource);
    }

    public void ChangeMusicVolume(float _change)
    {
        ChangeSourceVolume(1, "musicVolume", _change, musicSource);
    }

    private void ChangeSourceVolume(float baseVolume, string volumeName, float change, AudioSource source)
    {
        // get initial value of volume and change it
        float currentVolume = PlayerPrefs.GetFloat(volumeName, 1);
        currentVolume += change;

        //check if  we reached the maxium or minium value
        if (currentVolume > 1)
        {
            currentVolume = 0;
        }
        else if (currentVolume < 0)
        {
            currentVolume = 1;
        }

        //assign final value
        float finalVolume = currentVolume * baseVolume;
        source.volume = finalVolume;

        //save final value to player prefs
        PlayerPrefs.SetFloat(volumeName, finalVolume);
    }

    public void SetSoundVolume(float volume)
    {
        ChangeSourceVolume(1, "soundVolume", volume - soundSource.volume, soundSource);
    }

    public void SetMusicVolume(float volume)
    {
        ChangeSourceVolume(1, "musicVolume", volume - musicSource.volume, musicSource);
    }

    public float GetSoundVolume()
    {
        return soundSource.volume;
    }

    public float GetMusicVolume()
    {
        return musicSource.volume;
    }
}