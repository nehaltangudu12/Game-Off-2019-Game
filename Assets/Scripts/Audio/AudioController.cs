using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance = null;

    private AudioSource _sfxSource;
    private AudioSource _backSource;
    private AudioListener _audioListener;

    private void Awake ()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType<AudioController> ();
        }

        Init ();
    }

    void Init ()
    {
        _sfxSource = this.gameObject.AddComponent<AudioSource> ();
        _backSource = this.gameObject.AddComponent<AudioSource> ();
        _audioListener = this.gameObject.AddComponent<AudioListener> ();

        _sfxSource.playOnAwake = false;
        _backSource.playOnAwake = false;

        _sfxSource.loop = true;
        _backSource.loop = true;
    }

    public void PlayMusic (AudioClip clip, float volume)
    {
        _backSource.clip = clip;
        _backSource.volume = volume;
        _backSource.Play ();
    }

    public void PlaySfx (AudioClip clip, float volume)
    {
        _sfxSource.PlayOneShot (clip, volume);
    }
}