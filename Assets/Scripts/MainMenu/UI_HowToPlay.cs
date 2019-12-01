using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UI_HowToPlay : MonoBehaviour
{
    [SerializeField] private VideoPlayer[] Movies = null;
    [SerializeField] private VideoClip[] Clips = null;
    [SerializeField] private Button CloseBtn = null;

    private MainMenuController _menuControl;
    private static string _path = Application.streamingAssetsPath;
    private string _movePath = string.Format ("{0}/Move/Move.mp4", _path);
    private string _jumpPath = string.Format ("{0}/Jump/Jump.mp4", _path);
    private string _cameraPath = string.Format ("{0}/Camera/Camera.mp4", _path);

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
            Movies[i].source = VideoSource.Url;

            if (i == 0)
            {
                Movies[i].url = _movePath;
            }
            else if (i == 1)
            {
                Movies[i].url = _jumpPath;
            }
            else if (i == 2)
            {
                Movies[i].url = _cameraPath;
            }
        }
    }

    void InitVideosForStandalone ()
    {
        for (int i = 0; i < Movies.Length; i++)
        {
            Movies[i].source = VideoSource.VideoClip;
            Movies[i].clip = Clips[i];
        }
    }

    internal void Close ()
    {
        this.gameObject.SetActive (false);
    }
}