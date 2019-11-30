using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : Singleton<MainMenuController>
{
    [SerializeField] private UI_Settings Settings = null;
    [SerializeField] private UI_HowToPlay HowToPlay = null;

    public bool IsSettingsActive { get; private set; }

    private void Start ()
    {
        Settings.Init(this);
        HowToPlay.Init(this);
    }

    internal void OpenSettings ()
    {
        Settings.gameObject.SetActive (true);
        IsSettingsActive = true;
    }

    internal void OpenHTP ()
    {
        HowToPlay.gameObject.SetActive (true);
        IsSettingsActive = true;
    }

    internal void CloseSettings ()
    {
        Settings.Close ();
        IsSettingsActive = false;
    }

    internal void CloseHTP ()
    {
        HowToPlay.Close ();
        IsSettingsActive = false;
    }
}