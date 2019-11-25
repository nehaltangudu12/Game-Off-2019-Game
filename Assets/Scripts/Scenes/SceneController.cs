using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    [SerializeField] private AudioClip BackgroundMusic = null;

    private AudioController _audioControl;

    private void Start ()
    {
        _audioControl = AudioController.Instance;
    }

    public void LoadSceneAsync (int sceneId)
    {
        SceneManager.LoadSceneAsync (1, LoadSceneMode.Single);

        _audioControl.PlayMusic (BackgroundMusic, .01f);

    }

    public void ResetCurrentScene ()
    {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
    }
}