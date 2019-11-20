using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public CameraController CameraTest;
    public TileMapController TilesController;

    void Start()
    {
        TilesController.Init();
        CameraTest.Init();
    }
}
