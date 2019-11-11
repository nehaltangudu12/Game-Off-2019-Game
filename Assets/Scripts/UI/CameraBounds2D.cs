using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBounds2D : MonoBehaviour
{
    [SerializeField] public Vector2 maxXlimit;
    [SerializeField] public Vector2 maxYlimit;

    private Camera _camera;
    public Camera Cam => _camera;
    
    void Start ()
    {
        TryGetComponent (out _camera);
    }

    public void Update ()
    {
        CalculateBounds ();
    }

    public void CalculateBounds ()
    {
        var pos = _camera.transform.position;
        float cameraHalfWidth = _camera.aspect * _camera.orthographicSize;
        maxXlimit = new Vector2 (pos.x + cameraHalfWidth, pos.x - cameraHalfWidth);
        maxYlimit = new Vector2 (pos.y + _camera.orthographicSize, pos.y - _camera.orthographicSize);
    }
}