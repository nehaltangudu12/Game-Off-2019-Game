    using System.Linq;
    using System;
    using DG.Tweening;
    using GhAyoub.InputSystem;
    using UnityEngine.UI;
    using UnityEngine;

    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Image CameraFrame;
        [SerializeField] private Image CameraBatteryTube;
        [SerializeField] private float TimeToSnap = 0.25f;
        [SerializeField] private float CamMoveStep = 1.5f;
        [SerializeField] private Sprite CameraHandGrab;
        [SerializeField] private Sprite CameraHandNormal;

        [SerializeField] private Sprite CameraFrameNormal;
        [SerializeField] private Sprite CameraFrameZoomedOut;
        [SerializeField] private CameraBounds2D CameraBounds;
        [SerializeField] private CameraEffects CameraEffects;

        private bool _isZoomedOut = false;
        private float _zoomInOrthoSize = 12f;
        private float _zoomOutOrthoSize = 24f;
        private Camera _mainCam;
        private Grid _tilesMapGrid;
        private InputData _inputData;

        private Tween _batteryTween = null;
        private PlayerControllerDuplicate _player;

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

            // MatrixGizmos ();

            FocusAreaGizmos ();
        }

        private Vector2 maxXlimit;
        private Vector2 maxYlimit;
        private Vector3 _initPos;

        void Update ()
        {
            BatteryChecker ();

            CameraBoundsMovement ();
        }

        void CameraBoundsMovement ()
        {
            if (_inputData.CameraZoomOut)
            {
                CameraZoom (false);
            }

            if (_isZoomedOut && _inputData.CameraZoomOutHold)
            {
                if (_inputData.CamGrab)
                {
                    Cursor.SetCursor (CameraHandGrab.texture, Vector2.zero, CursorMode.Auto);
                    var mPos = _inputData.MousePosition;
                    CameraBounds.transform.DOMove (new Vector3 (mPos.x, mPos.y, -200), TimeToSnap * Time.unscaledDeltaTime);
                }
                else
                {
                    Cursor.SetCursor (CameraHandNormal.texture, Vector2.zero, CursorMode.Auto);
                }

                if (_inputData.CamArrowUp)
                {
                    var max = _player.transform.position.y + _zoomInOrthoSize;

                    if (max > _zoomInOrthoSize) max = _zoomInOrthoSize;

                    CameraBounds.transform.DOMoveY (Mathf.Clamp (CameraBounds.transform.position.y + CamMoveStep, -_zoomInOrthoSize, max), TimeToSnap * Time.unscaledDeltaTime);
                }
                else if (_inputData.CamArrowDown)
                {
                    var min = (_player.transform.position.y + 3) - _zoomInOrthoSize;

                    if (min < -_zoomInOrthoSize) min = -_zoomInOrthoSize;

                    CameraBounds.transform.DOMoveY (Mathf.Clamp (CameraBounds.transform.position.y - CamMoveStep, min, _zoomInOrthoSize), TimeToSnap * Time.unscaledDeltaTime);
                }
                else if (_inputData.CamArrowLeft)
                {
                    var min = _player.transform.position.x - 21f;

                    if (min < -21f) min = -21f;

                    CameraBounds.transform.DOMoveX (Mathf.Clamp (CameraBounds.transform.position.x - CamMoveStep, min, 21f), TimeToSnap * Time.unscaledDeltaTime);
                }
                else if (_inputData.CamArrowRight)
                {
                    var max = _player.transform.position.x + 21f;

                    if (max > 21f) max = 21f;

                    CameraBounds.transform.DOMoveX (Mathf.Clamp (CameraBounds.transform.position.x + CamMoveStep, -21f, max), TimeToSnap * Time.unscaledDeltaTime);
                }
            }
            else if (_inputData.CameraZoomIn)
            {
                CameraZoom (true);
            }
        }

        void CameraZoom (bool zoomIn)
        {
            _isZoomedOut = !zoomIn;
            Cursor.visible = !zoomIn;
            Time.timeScale = zoomIn ? 1f : 0.01f;
            CameraFrame.sprite = zoomIn ? CameraFrameNormal : CameraFrameZoomedOut;

            CameraEffects.LensDistortionStatus (zoomIn);

            TweenBattery (!zoomIn);

            var pos = new Vector3 (0, 0, -200f);
            transform.DOMove (zoomIn ? CameraBounds.transform.position : pos, TimeToSnap * Time.unscaledDeltaTime, true);
            _mainCam.DOOrthoSize (zoomIn ? _zoomInOrthoSize : _zoomOutOrthoSize, TimeToSnap * Time.unscaledDeltaTime);
        }

        void TweenBattery (bool emptyIt)
        {
            _batteryTween?.Kill ();

            if (emptyIt)
            {
                _batteryTween = CameraBatteryTube.DOFillAmount (0, 4f * Time.unscaledDeltaTime);

                DOTween.Play (_batteryTween);
            }
            else
            {
                _batteryTween = CameraBatteryTube.DOFillAmount (1, 4f);

                _batteryTween.Play ();
            }
        }

        void BatteryChecker ()
        {
            if (CameraBatteryTube.fillAmount <= 0.15f)
            {
                if (_isZoomedOut) CameraZoom (true);
            }

            if (CameraBatteryTube.fillAmount < 0.3f)
            {
                CameraBatteryTube.color = Color.red;
            }
            else if (CameraBatteryTube.fillAmount < 0.7f)
            {
                CameraBatteryTube.color = Color.yellow;
            }
            else
            {
                CameraBatteryTube.color = Color.green;
            }
        }
    }