using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausemenuUI;

    public static bool IsPaused = false;
    private SceneController _sceneControl;

    private void Start ()
    {
        _sceneControl = SceneController.Instance;
    }

    void Update ()
    {
        if (Input.GetKeyDown (KeyCode.Escape))
        {
            if (IsPaused)
            {
                Resume ();
            }
            else
            {
                Pause ();
            }
        }
    }

    public void Resume ()
    {
        pausemenuUI.SetActive (false);
        Time.timeScale = 1f;
        IsPaused = false;
    }

    void Pause ()
    {
        pausemenuUI.SetActive (true);
        Time.timeScale = 0.0f;
        IsPaused = true;
    }

    public void LoadMenu ()
    {
        SceneManager.LoadScene ("MainMenu");
        Time.timeScale = 1f;
    }

    public void Restart ()
    {
        _sceneControl.ResetCurrentScene();
        Resume ();
    }
}