using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Menu Audio")]
    public AudioClip menuMusic;
    public AudioClip buttonHover;
    public AudioClip buttonClick;

    void Start()
    {
        PlayMenuMusic();
    }

    public void PlayMenuMusic()
    {
        if (menuMusic == null)
            return;

        musicSource.clip = menuMusic;
        musicSource.Play();
    }

    public void PlayHover()
    {
        if (buttonHover == null)
            return;

        sfxSource.PlayOneShot(buttonHover);
    }

    public void PlayClick()
    {
        if (buttonClick == null)
            return;

        sfxSource.PlayOneShot(buttonClick);
    }
}