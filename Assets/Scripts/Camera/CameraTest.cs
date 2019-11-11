    using DG.Tweening;
    using GhAyoub.InputSystem;
    using UnityEngine;

    public class CameraTest : MonoBehaviour
    {
        [SerializeField] private float TimeToSnap = 0.25f;
        [SerializeField] private float CamMoveStep = 1.5f;
        [SerializeField] private CameraBounds2D CameraBounds;

        private bool _isZoomedOut = false;
        private Camera _mainCam;
        private Grid _tilesMapGrid;
        private InputData _inputData;

        public void Init ()
        {
            TryGetComponent (out _mainCam);

            _tilesMapGrid = TileMapController.Instance.TilesGrid;
            _inputData = PlayerInput.Instance.Data;
        }

        void OnDrawGizmos ()
        {
            if (!_isZoomedOut) return;
            Gizmos.color = Color.green;

            var max1 = new Vector3 (CameraBounds.maxXlimit.x, CameraBounds.maxYlimit.x);
            var max2 = new Vector3 (CameraBounds.maxXlimit.y, CameraBounds.maxYlimit.y);

            Gizmos.DrawSphere (max1, 0.5f);
            Gizmos.DrawSphere (max2, 0.5f);

            Gizmos.color = Color.black;

            Gizmos.DrawLine (max1, max2);
        }

        private int index = 0;
        void Update ()
        {
            if (Input.GetKeyDown (KeyCode.F2))  
            {
                _isZoomedOut = true;

                var pos = new Vector3 (0f, 0f, -200f);
                transform.DOMove (pos, TimeToSnap);
                _mainCam.DOOrthoSize (24f, TimeToSnap);
            }

            if (Input.GetKey (KeyCode.F2))
            {
                if (_inputData.CamArrowUp)
                {
                    CameraBounds.transform.DOMoveY (Mathf.Clamp (CameraBounds.transform.position.y + CamMoveStep, -12f, 12f), TimeToSnap);
                }
                else if (_inputData.CamArrowDown)
                {
                    CameraBounds.transform.DOMoveY (Mathf.Clamp (CameraBounds.transform.position.y - CamMoveStep, -12f, 12f), TimeToSnap);
                }
                else if (_inputData.CamArrowLeft)
                {
                    CameraBounds.transform.DOMoveX (Mathf.Clamp (CameraBounds.transform.position.x - CamMoveStep, -21f, 21f), TimeToSnap);
                }
                else if (_inputData.CamArrowRight)
                {
                    CameraBounds.transform.DOMoveX (Mathf.Clamp (CameraBounds.transform.position.x + CamMoveStep, -21f, 21f), TimeToSnap);
                }
            }

            if (Input.GetKeyUp (KeyCode.F2))
            {
                _isZoomedOut = false;

                var pos0 = _tilesMapGrid.CellToWorld (new Vector3Int (0, 0, 0));
                var posing0 = new Vector3 (pos0.x - 21f, pos0.y - 12f, -200);
                transform.DOMove (CameraBounds.transform.position, TimeToSnap, true);
                _mainCam.DOOrthoSize (12f, TimeToSnap);
            }

            // if (Input.GetKeyDown (KeyCode.F1))
            // {
            //     var pos0 = _tilesMapGrid.CellToWorld (new Vector3Int (0, 0, 0));
            //     var pos1 = _tilesMapGrid.CellToWorld (new Vector3Int (0, 1, 0));
            //     var pos2 = _tilesMapGrid.CellToWorld (new Vector3Int (1, 0, 0));
            //     var pos3 = _tilesMapGrid.CellToWorld (new Vector3Int (1, 1, 0));

            //     switch (index)
            //     {
            //         case 0:
            //             var posing = new Vector3 (pos0.x - 21f, pos0.y - 12f, -200);
            //             transform.DOMove (posing, TimeToSnap, true);
            //             break;

            //         case 1:
            //             var posing1 = new Vector3 (pos1.x - 21f, pos1.y - 12f, -200);
            //             transform.DOMove (posing1, TimeToSnap, true);
            //             break;

            //         case 2:
            //             var posing2 = new Vector3 (pos2.x - 21f, pos2.y - 12f, -200);
            //             transform.DOMove (posing2, TimeToSnap, true);
            //             break;

            //         case 3:
            //             var posing3 = new Vector3 (pos3.x / 2, pos3.y / 2, -200);
            //             transform.DOMove (posing3, TimeToSnap, true);
            //             break;

            //         default:
            //             index = 0;
            //             var defaultPos = new Vector3 (pos0.x - 21f, pos0.y - 12f, -200);
            //             transform.DOMove (defaultPos, TimeToSnap, true);
            //             break;
            //     }

            //     _mainCam.DOOrthoSize (12f, TimeToSnap);

            //     index++;
            // }
        }
    }