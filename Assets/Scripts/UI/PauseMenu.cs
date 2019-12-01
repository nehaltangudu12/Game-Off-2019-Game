using System.Collections;
using System.Collections.Generic;
using GhAyoub.InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausemenuUI;

    public static bool IsPaused = false;
    private PlayerInput _playerInput;
    private SceneController _sceneControl;

    private void Start ()
    {
        _sceneControl = SceneController.Instance;
        _playerInput = PlayerInput.Instance;
    }

    void Update ()
    {
        if (_playerInput.Data.PauseMenu)
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
        IsPaused = false;
        Cursor.visible = false;
        pausemenuUI.SetActive (false);
        Time.timeScale = 1f;
    }

    void Pause ()
    {
        IsPaused = true;
        Cursor.visible = true;
        pausemenuUI.SetActive (true);
        Time.timeScale = 0.0f;
    }

    public void LoadMenu ()
    {
        Time.timeScale = 1f;
        _sceneControl.LoadSceneAsync (0);
    }

    public void Restart ()
    {
        _sceneControl.ResetCurrentScene ();
        Resume ();
    }
}