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
        private PlayerControllerDuplicate _player;

        private float _zoomOutOrthoSize = 24f;
        private float _zoomInOrthoSize = 11.8f;

        public void Init ()
        {
            TryGetComponent (out _mainCam);

            _tilesMapGrid = TileMapController.Instance.TilesGrid;
            _inputData = PlayerInput.Instance.Data;

            CalculateGridForGizmos ();
        }

        public void Init (PlayerControllerDuplicate player)
        {
            _player = player;
        }

        void CalculateGridForGizmos ()
        {
            _initPos = transform.position;
            float cameraHalfWidth = _mainCam.aspect * 24f;
            maxXlimit = new Vector2 (_initPos.x + cameraHalfWidth, _initPos.x - cameraHalfWidth);
            maxYlimit = new Vector2 (_initPos.y + 24f, _initPos.y - 24f);
        }

        void MatrixGizmos ()
        {
            Gizmos.color = Color.red;

            for (int i = -21; i < (int) Mathf.Abs (maxXlimit.y); i++)
            {
                for (int j = -12; j < (int) Mathf.Abs (maxYlimit.y); j++)
                {
                    Gizmos.DrawWireCube (new Vector2 (_initPos.x + i + .5f, _initPos.y + j + .5f), Vector3.one);
                }
            }
        }

        void FocusAreaGizmos ()
        {
            Gizmos.color = Color.green;

            var min1 = new Vector3 (CameraBounds.maxXlimit.x, CameraBounds.maxYlimit.x);
            var max1 = new Vector3 (CameraBounds.maxXlimit.x, CameraBounds.maxYlimit.y);

            var min2 = new Vector3 (CameraBounds.maxXlimit.y, CameraBounds.maxYlimit.x);
            var max2 = new Vector3 (CameraBounds.maxXlimit.y, CameraBounds.maxYlimit.y);

            Gizmos.DrawSphere (min1, 0.5f);
            Gizmos.DrawSphere (max1, 0.5f);

            Gizmos.DrawSphere (min2, 0.5f);
            Gizmos.DrawSphere (max2, 0.5f);

            Gizmos.color = Color.black;

            Gizmos.DrawLine (max1, min2);
            Gizmos.DrawLine (max1, max2);
            Gizmos.DrawLine (max1, min1);
            Gizmos.DrawLine (max2, min2);
            Gizmos.DrawLine (max2, min1);
            Gizmos.DrawLine (min2, min1);
        }

        void OnDrawGizmos ()
        {
            if (!_isZoomedOut) return;

            MatrixGizmos ();

            FocusAreaGizmos ();
        }

        private Vector2 maxXlimit;
        private Vector2 maxYlimit;
        private Vector3 _initPos;

        void Update ()
        {
            if (Input.GetKeyDown (KeyCode.F2))
            {
                _isZoomedOut = true;

                var pos = new Vector3 (0, 0, -200f);
                transform.DOMove (pos, TimeToSnap);
                _mainCam.DOOrthoSize (_zoomOutOrthoSize, TimeToSnap);
            }

            if (Input.GetKey (KeyCode.F2))
            {
                if (_inputData.CamArrowUp)
                {
                    var max = _player.transform.position.y + _zoomInOrthoSize;

                    if (max > _zoomInOrthoSize) max = _zoomInOrthoSize;

                    CameraBounds.transform.DOMoveY (Mathf.Clamp (CameraBounds.transform.position.y + CamMoveStep, -_zoomInOrthoSize, max), TimeToSnap);
                }
                else if (_inputData.CamArrowDown)
                {
                    var min = _player.transform.position.y - _zoomInOrthoSize;

                    if (min < -_zoomInOrthoSize) min = -_zoomInOrthoSize;

                    CameraBounds.transform.DOMoveY (Mathf.Clamp (CameraBounds.transform.position.y - CamMoveStep, min, _zoomInOrthoSize), TimeToSnap);
                }
                else if (_inputData.CamArrowLeft)
                {
                    var min = _player.transform.position.x - 21f;

                    if (min < -21f) min = -21f;

                    CameraBounds.transform.DOMoveX (Mathf.Clamp (CameraBounds.transform.position.x - CamMoveStep, min, 21f), TimeToSnap);
                }
                else if (_inputData.CamArrowRight)
                {
                    var max = _player.transform.position.x + 21f;

                    if (max > 21f) max = 21f;

                    CameraBounds.transform.DOMoveX (Mathf.Clamp (CameraBounds.transform.position.x + CamMoveStep, -21f, max), TimeToSnap);
                }
            }

            if (Input.GetKeyUp (KeyCode.F2))
            {
                _isZoomedOut = false;

                var pos0 = _tilesMapGrid.CellToWorld (new Vector3Int (0, 0, 0));
                var posing0 = new Vector3 (pos0.x - 21f, pos0.y - _zoomInOrthoSize, -200);
                transform.DOMove (CameraBounds.transform.position, TimeToSnap, true);
                _mainCam.DOOrthoSize (_zoomInOrthoSize, TimeToSnap);
            }
        }
    }