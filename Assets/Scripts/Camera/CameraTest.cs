using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraTest : MonoBehaviour
{
    private Camera _camera;
    public Grid _tileMap;
    void Start ()
    {
        TryGetComponent (out _camera);
    }

    int index = 0;
    void Update ()
    {
        if (Input.GetKeyDown (KeyCode.F2))
        {
            var pos = new Vector3 (0f, 0f, -200f);
            transform.DOMove (pos, 2f);
            _camera.DOOrthoSize (24f, 2f);
        }

        if (Input.GetKeyUp (KeyCode.F2))
        {
            var pos0 = _tileMap.CellToWorld (new Vector3Int (0, 0, 0));
            var posing0 = new Vector3 (pos0.x - 21f, pos0.y - 12f, -200);
            transform.DOMove (posing0, 2f);
            _camera.DOOrthoSize (12f, 2f);
        }

        if (Input.GetKeyDown (KeyCode.F1))
        {
            var pos0 = _tileMap.CellToWorld (new Vector3Int (0, 0, 0));
            var pos1 = _tileMap.CellToWorld (new Vector3Int (0, 1, 0));
            var pos2 = _tileMap.CellToWorld (new Vector3Int (1, 0, 0));
            var pos3 = _tileMap.CellToWorld (new Vector3Int (1, 1, 0));

            switch (index)
            {
                case 0:
                    var posing = new Vector3 (pos0.x - 21f, pos0.y - 12f, -200);
                    transform.DOMove (posing, 2f);

                    break;

                case 1:
                    var posing1 = new Vector3 (pos1.x - 21f, pos1.y - 12f, -200);
                    transform.DOMove (posing1, 2f);

                    break;

                case 2:
                    var posing2 = new Vector3 (pos2.x - 21f, pos2.y - 12f, -200);
                    transform.DOMove (posing2, 2f);
                    break;

                case 3:
                    var posing3 = new Vector3 (pos3.x / 2, pos3.y / 2, -200);
                    transform.DOMove (posing3, 2f);
                    break;

                default:
                    index = -1;
                    break;
            }
            _camera.DOOrthoSize (12f, 2f);

            index++;
        }
    }
}