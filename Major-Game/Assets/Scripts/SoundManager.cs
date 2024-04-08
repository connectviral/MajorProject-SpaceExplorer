using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource sfxSource;

    private bool musicMuted = false;
    private bool sfxMuted = false;

    public void ToggleMusic()
    {
        musicMuted = !musicMuted;
        musicSource.mute = musicMuted;
    }

    public void ToggleSoundEffects()
    {
        sfxMuted = !sfxMuted;
        sfxSource.mute = sfxMuted;
    }
}
