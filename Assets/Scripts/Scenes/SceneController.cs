using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : SingletonMB<SceneController>
{
    [SerializeField] private AudioClip BackgroundMusic = null;
    [SerializeField] private AudioClip MMBackgroundMusic = null;

    private AudioController _audioControl;

    private void Start ()
    {
        _audioControl = AudioController.Instance;

        _audioControl.PlayMusic (MMBackgroundMusic, .5f);
    }

    public void LoadSceneAsync (int sceneId)
    {
        SceneManager.LoadSceneAsync (sceneId, LoadSceneMode.Single);

        if (sceneId > 0)
        {
            Cursor.visible = false;
            _audioControl.PlayMusic (BackgroundMusic, .5f);
        }
        else
        {
            Cursor.visible = true;
            _audioControl.PlayMusic (MMBackgroundMusic, .5f);
        }
    }

    public void ResetCurrentScene ()
    {
        LoadSceneAsync (SceneManager.GetActiveScene ().buildIndex);
    }
}