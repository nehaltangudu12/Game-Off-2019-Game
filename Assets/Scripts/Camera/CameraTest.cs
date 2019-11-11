    using DG.Tweening;
    using GhAyoub.InputSystem;
    using UnityEngine;

    public class CameraTest : MonoBehaviour
    {
        [SerializeField] private float TimeToSnap = 0.25f;
        [SerializeField] private float CamMoveStep = 1.5f;

        private Camera _camera;

        private Grid _tilesMapGrid;
        private InputData _inputData;

        public void Init ()
        {
            TryGetComponent (out _camera);

            _tilesMapGrid = TileMapController.Instance.TilesGrid;
            _inputData = PlayerInput.Instance.Data;
        }

        private int index = 0;
        void Update ()
        {
            if (_inputData.CamArrowUp)
            {
                transform.DOMoveY (Mathf.Clamp (transform.position.y + CamMoveStep, -12f, 12f), TimeToSnap);
            }
            else if (_inputData.CamArrowDown)
            {
                transform.DOMoveY (Mathf.Clamp (transform.position.y - CamMoveStep, -12f, 12f), TimeToSnap);
            }
            else if (_inputData.CamArrowLeft)
            {
                transform.DOMoveX (Mathf.Clamp (transform.position.x - CamMoveStep, -21f, 21f), TimeToSnap);
            }
            else if (_inputData.CamArrowRight)
            {
                transform.DOMoveX (Mathf.Clamp (transform.position.x + CamMoveStep, -21f, 21f), TimeToSnap);
            }

            if (Input.GetKeyDown (KeyCode.F2))
            {
                var pos = new Vector3 (0f, 0f, -200f);
                transform.DOMove (pos, TimeToSnap);
                _camera.DOOrthoSize (24f, TimeToSnap);
            }

            if (Input.GetKeyUp (KeyCode.F2))
            {
                var pos0 = _tilesMapGrid.CellToWorld (new Vector3Int (0, 0, 0));
                var posing0 = new Vector3 (pos0.x - 21f, pos0.y - 12f, -200);
                transform.DOMove (posing0, TimeToSnap, true);
                _camera.DOOrthoSize (12f, TimeToSnap);
            }

            if (Input.GetKeyDown (KeyCode.F1))
            {
                var pos0 = _tilesMapGrid.CellToWorld (new Vector3Int (0, 0, 0));
                var pos1 = _tilesMapGrid.CellToWorld (new Vector3Int (0, 1, 0));
                var pos2 = _tilesMapGrid.CellToWorld (new Vector3Int (1, 0, 0));
                var pos3 = _tilesMapGrid.CellToWorld (new Vector3Int (1, 1, 0));

                switch (index)
                {
                    case 0:
                        var posing = new Vector3 (pos0.x - 21f, pos0.y - 12f, -200);
                        transform.DOMove (posing, TimeToSnap, true);
                        break;

                    case 1:
                        var posing1 = new Vector3 (pos1.x - 21f, pos1.y - 12f, -200);
                        transform.DOMove (posing1, TimeToSnap, true);
                        break;

                    case 2:
                        var posing2 = new Vector3 (pos2.x - 21f, pos2.y - 12f, -200);
                        transform.DOMove (posing2, TimeToSnap, true);
                        break;

                    case 3:
                        var posing3 = new Vector3 (pos3.x / 2, pos3.y / 2, -200);
                        transform.DOMove (posing3, TimeToSnap, true);
                        break;

                    default:
                        index = 0;
                        var defaultPos = new Vector3 (pos0.x - 21f, pos0.y - 12f, -200);
                        transform.DOMove (defaultPos, TimeToSnap, true);
                        break;
                }

                _camera.DOOrthoSize (12f, TimeToSnap);

                index++;
            }
        }
    }