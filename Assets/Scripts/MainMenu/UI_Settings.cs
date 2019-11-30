using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Settings : MonoBehaviour
{
    [SerializeField] private Slider MasterSlider = null;
    [SerializeField] private Slider MusicSlider = null;
    [SerializeField] private Slider SfxSlider = null;
    [SerializeField] private Button CloseBtn = null;

    private AudioController _audioControl;
    private MainMenuController _menuControl;

    internal void Init (MainMenuController menuControl)
    {
        _menuControl = menuControl;
        _audioControl = AudioController.Instance;

        CloseBtn.onClick.AddListener (() =>
        {
            _menuControl.CloseSettings ();
        });

        MasterSlider.onValueChanged.AddListener (el =>
        {
            _audioControl.ChangeMasterVolume (el);
        });

        MusicSlider.onValueChanged.AddListener (el =>
        {
            _audioControl.ChangeMusicVolume (el);
        });

        SfxSlider.onValueChanged.AddListener (el =>
        {
            _audioControl.ChangeSfxVolume (el);
        });

        _audioControl.InitSliders (MasterSlider, MusicSlider, SfxSlider);
    }

    internal void Close ()
    {
        this.gameObject.SetActive (false);
    }
}