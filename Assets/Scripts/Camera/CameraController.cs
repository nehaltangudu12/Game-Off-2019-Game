    using System.Linq;
    using System;
    using DG.Tweening;
    using GhAyoub.InputSystem;
    using UnityEngine.UI;
    using UnityEngine;

    public class CameraController : Singleton<CameraController>
    {
        [SerializeField] private float CamMoveStep = 1.5f;
        [SerializeField] private float TimeToSnap = 0.25f;
        [SerializeField] private float CamTransition = 1.5f;
        [SerializeField] private Image CameraFlashScreen;
        [SerializeField] private Texture2D CameraHandGrab;
        [SerializeField] private Texture2D CameraHandNormal;
        [SerializeField] private Transform CameraScreenTrans;

        [SerializeField] private Battery CameraBattery;
        [SerializeField] private CameraBounds2D CameraBounds;
        [SerializeField] private CameraEffects CameraEffects;

        private bool _isZoomedOut = false;
        private float _zoomInOrthoSize = 12f;
        private float _zoomOutOrthoSize = 24f;
        private Camera _mainCam;
        private Grid _tilesMapGrid;
        private InputData _inputData;

        private Sequence _zoomTweenSeq = null;
        private CharacterController _player;

        public float PreviousFillAmount { get; private set; } = 1f;
        public Vector3 BoundsWorldPos => CameraBounds.Position;

        public Property<bool> IsZoomedOut = new Property<bool> ();

        public Property<bool> BatteryStatus = new Property<bool> ();

        public void Init ()
        {
            TryGetComponent (out _mainCam);

            _tilesMapGrid = TileMapController.Instance.TilesGrid;
            _inputData = PlayerInput.Instance.Data;

            _zoomTweenSeq = DOTween.Sequence ();

            CalculateGridForGizmos ();
        }

        public void Init (CharacterController player)
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
            CameraBoundsMovement ();
        }

        void CameraBoundsMovement ()
        {
            if (_inputData.CameraZoomOut)
            {
                CameraZoom (false);
                CameraScreenShot ();
            }

            if (_isZoomedOut && _inputData.CameraZoomOutHold)
            {
                if (_inputData.CamGrab)
                {
                    Cursor.SetCursor (CameraHandGrab, Vector2.zero, CursorMode.Auto);
                    var mPos = _inputData.MousePosition;
                    CameraBounds.transform.DOMove (new Vector3 (mPos.x, mPos.y, -200), TimeToSnap * Time.unscaledDeltaTime);
                }
                else
                {
                    Cursor.SetCursor (CameraHandNormal, Vector2.zero, CursorMode.Auto);
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

            if (_inputData.CameraZoomIn)
            {
                CameraZoom (true);
            }
        }

        private Texture2D _lensTex = null;
        void CameraScreenShot ()
        {
            var rt = new RenderTexture (Screen.width, Screen.height, 24);
            _mainCam.targetTexture = rt;
            _lensTex = new Texture2D (Screen.width, Screen.height, TextureFormat.RGB24, false);
            CameraEffects.LensDistortionStatus (true, 60);
            _mainCam.Render ();
            RenderTexture.active = rt;
            _lensTex.ReadPixels (new Rect (0, 0, Screen.width, Screen.height), 0, 0);
            _lensTex.Apply ();
            _mainCam.targetTexture = null;
            CameraEffects.LensDistortionStatus (false);

            CameraBounds.Lens.texture = _lensTex;
        }

        void CameraFlash ()
        {
            CameraFlashScreen.DOFade (1.0f, 0.05f).SetEase (Ease.Flash).OnComplete (() =>
            {
                CameraFlashScreen.DOFade (0.0f, 0.05f).SetEase (Ease.Flash);
            });
        }

        void CameraZoom (bool zoomIn)
        {
            _isZoomedOut = !zoomIn;
            Cursor.visible = !zoomIn;
            CameraBattery.CacheBatteryAmount ();
            Time.timeScale = zoomIn ? 1f : 0.01f;

            CameraEffects.LensDistortionStatus (zoomIn);

            IsZoomedOut.Value = _isZoomedOut;

            var lensPos = CameraBounds.Lens.transform.position;
            var pos = new Vector3 (lensPos.x, lensPos.y, -200f);

            _zoomTweenSeq?.Kill ();

            if (zoomIn)
            {
                CameraFlash ();

                _zoomTweenSeq.Append (transform.DOMoveY (lensPos.y, CamTransition * 3f).OnComplete (() =>
                {
                    _mainCam.DOOrthoSize (7f, CamTransition).OnComplete (() =>
                    {
                        transform.DOMove (CameraBounds.transform.position, 0.005f).SetEase (Ease.Flash);
                        _mainCam.DOOrthoSize (_zoomInOrthoSize, 0.01f);

                        // UI Callback
                        BatteryStatus.Fire (false);
                    });
                }));
            }
            else
            {
                _zoomTweenSeq.Append (transform.DOMove (pos, 0.01f * Time.unscaledDeltaTime, true));

                _zoomTweenSeq.Append (_mainCam.DOOrthoSize (_zoomOutOrthoSize, CamTransition * Time.unscaledDeltaTime).OnComplete (() =>
                {
                    transform.DOMoveY (CameraScreenTrans.position.y, CamTransition * Time.unscaledDeltaTime, false);

                    // UI Callback
                    BatteryStatus.Fire (true);
                }));
            }
        }
    }