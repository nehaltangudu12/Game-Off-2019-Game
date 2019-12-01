using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UI_HowToPlay : MonoBehaviour
{
    [SerializeField] private VideoPlayer[] Movies = null;
    [SerializeField] private Button CloseBtn = null;

    private MainMenuController _menuControl;
    private string _path = Application.streamingAssetsPath;

    internal void Init (MainMenuController menuControl)
    {
        _menuControl = menuControl;

        CloseBtn.onClick.AddListener (() =>
        {
            _menuControl.CloseHTP ();
        });

        InitVideosUrls ();
    }

    void InitVideosUrls ()
    {
        for (int i = 0; i < Movies.Length; i++)
        {
            if (i == 0)
            {
                Movies[i].url = string.Format ("{0}/Move/move.mp4", _path);
            }
            else if (i == 1)
            {
                Movies[i].url = string.Format ("{0}/Jump/jump.mp4", _path);
            }
            else if (i == 2)
            {
                Movies[i].url = string.Format ("{0}/Camera/camera.mp4", _path);
            }
        }
    }

    internal void Close ()
    {
        this.gameObject.SetActive (false);
    }
}