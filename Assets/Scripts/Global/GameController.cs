using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public UIController UIControl;
    public CameraController CameraTest;
    public TileMapController TilesController;

    void Start()
    {
        UIControl.Init();
        CameraTest.Init();
        TilesController.Init();
    }
}
