using UnityEngine;
using UnityEngine.UI;

public class CameraBounds2D : MonoBehaviour
{
    [SerializeField] private RawImage _cameraLens;
    [HideInInspector] public Vector2 maxXlimit;
    [HideInInspector] public Vector2 maxYlimit;

    private Camera _camera;
    public Camera Cam => _camera;

    public RawImage Lens => _cameraLens;

    public Vector3 Position => this.transform.position;

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