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

    private AudioData _audioData;

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

        var data = SaveLoadManager.LoadData<AudioData> ();
        if (data == null)
        {
            _audioData = new AudioData ();
        }
        else
        {
            _audioData = data;

            ChangeMasterVolume (_audioData.MasterVolume);
            ChangeMusicVolume (_audioData.MusicVolume);
            ChangeSfxVolume (_audioData.SfxVolume);
        }
    }

    internal void InitSliders (Slider master, Slider music, Slider sfx)
    {
        master.value = _audioData.MasterVolume;
        music.value = _audioData.MusicVolume;
        sfx.value = _audioData.SfxVolume;
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
    }

    internal void ChangeMusicVolume (float volume)
    {
        AudioMixer.SetFloat ("mu", volume);
    }

    internal void ChangeSfxVolume (float volume)
    {
        AudioMixer.SetFloat ("sfx", volume);
    }

    internal void ChangeSlider (Slider slider, float volume)
    {
        slider.value = volume;
    }

    void OnApplicationQuit ()
    {

        AudioMixer.GetFloat ("m", out var m);
        AudioMixer.GetFloat ("mu", out var mu);
        AudioMixer.GetFloat ("sfx", out var sfx);

        _audioData.MasterVolume = m;
        _audioData.MusicVolume = mu;
        _audioData.SfxVolume = sfx;

        SaveLoadManager.SaveData (_audioData);
    }
}

[System.Serializable]
class AudioData
{
    public float MasterVolume;
    public float MusicVolume;
    public float SfxVolume;
}