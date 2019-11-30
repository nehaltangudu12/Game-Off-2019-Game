using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
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

        if (sceneId == 1)
            _audioControl.PlayMusic (BackgroundMusic, .5f);
        else
            _audioControl.PlayMusic (MMBackgroundMusic, .5f);
    }

    public void ResetCurrentScene ()
    {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
    }
}