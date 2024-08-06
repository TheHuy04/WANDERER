using UnityEngine;

public class MusicPlayer123 : MonoBehaviour
{
    public AudioSource introSource;
    public AudioSource loopSource;

    void Start()
    {
        if (introSource != null && introSource.clip != null)
        {
            // Play intro and schedule loop
            introSource.Play();
            loopSource.PlayScheduled(AudioSettings.dspTime + introSource.clip.length);
        }
        else
        {
            // No intro, just play the loop
            loopSource.Play();
        }
    }

    void Update()
    {
        // If we have an intro, check when it's finished to start the loop
        if (introSource != null && introSource.clip != null)
        {
            if (!introSource.isPlaying && !loopSource.isPlaying)
            {
                loopSource.Play();
            }
        }

        // Ensure loop keeps playing
        if (!loopSource.isPlaying)
        {
            loopSource.Play();
        }
    }
}