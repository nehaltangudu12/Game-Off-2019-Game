using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : SingletonMB<GameController>
{
    public UIController UIControl;
    public CameraController CameraTest;

    new void Awake ()
    {
        Application.targetFrameRate = 60;
    }

    void Start ()
    {
        UIControl.Init ();
        CameraTest.Init ();
    }
}