using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance = null;

    [SerializeField] private AudioMixer AudioMixer = null;
    [SerializeField] private AudioMixerGroup MasterGroup = null;
    [SerializeField] private AudioMixerGroup MusicGroup = null;
    [SerializeField] private AudioMixerGroup SfxGroup = null;

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

        _sfxSource.loop = false;
        _backSource.loop = true;

        _sfxSource.outputAudioMixerGroup = SfxGroup;
        _backSource.outputAudioMixerGroup = MusicGroup;

    }

    internal void InitSliders (Slider master, Slider music, Slider sfx)
    {
        var masterKey = PlayerPrefs.HasKey ("Master");
        var musicKey = PlayerPrefs.HasKey ("Music");
        var sfxKey = PlayerPrefs.HasKey ("Sfx");

        if (masterKey)
        {
            var masterVal = PlayerPrefs.GetFloat ("Master");
            ChangeMasterVolume (masterVal);
            master.value = masterVal;
        }

        if (musicKey)
        {
            var musicVal = PlayerPrefs.GetFloat ("Music");
            ChangeMusicVolume (musicVal);
            music.value = musicVal;
        }

        if (sfxKey)
        {
            var sfxVal = PlayerPrefs.GetFloat ("Sfx");
            ChangeSfxVolume (sfxVal);
            sfx.value = sfxVal;
        }
    }

    public void PlayMusic (AudioClip clip, float volume)
    {
        _backSource.clip = clip;
        //_backSource.volume = volume;
        _backSource.Play ();
    }

    public void PlaySfx (AudioClip clip, float volume)
    {
        _sfxSource.PlayOneShot (clip, volume);
    }

    internal void ChangeMasterVolume (float volume)
    {
        AudioMixer.SetFloat ("m", volume);
        Save ();
    }

    internal void ChangeMusicVolume (float volume)
    {
        AudioMixer.SetFloat ("mu", volume);
        Save ();
    }

    internal void ChangeSfxVolume (float volume)
    {
        AudioMixer.SetFloat ("sfx", volume);
        Save ();
    }

    internal void ChangeSlider (Slider slider, float volume)
    {
        slider.value = volume;
    }

    void Save ()
    {
        AudioMixer.GetFloat ("m", out var m);
        AudioMixer.GetFloat ("mu", out var mu);
        AudioMixer.GetFloat ("sfx", out var sfx);

        PlayerPrefs.SetFloat ("Master", m);
        PlayerPrefs.SetFloat ("Music", mu);
        PlayerPrefs.SetFloat ("Sfx", sfx);

        PlayerPrefs.Save ();
    }
}

[System.Serializable]
class AudioData
{
    public float MasterVolume;
    public float MusicVolume;
    public float SfxVolume;
}