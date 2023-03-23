using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance = null;
    public static AudioManager Instance
    {
        get { return instance; }
    }

    [SerializeField]
    private AudioSource sfxSource;

    [SerializeField]
    private AudioSource musicSource;

    AudioClip gameBackgroundMusic;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start() {
        PlayMusic(GameManager.Instance.audioFilesSO.backgroundMusic,0.15f);
    }

    public void PlaySFX(AudioClip clip, float volume = 1.0f)
    {
        sfxSource.volume = volume;
        sfxSource.PlayOneShot(clip);
    }

    private void PlayMusic(AudioClip clip, float volume = 1.0f)
    {
        if (musicSource.isPlaying && musicSource.clip == clip)
        {
            return;
        }

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.volume = volume;
        musicSource.Play();
    }
}
