using UnityEngine;

namespace GhAyoub.InputSystem
{
    public class InputData
    {
        public bool CameraZoomIn { get; internal set; }
        public bool CameraZoomOut { get; internal set; }
        public bool CameraZoomOutHold { get; internal set; }
        public bool PauseMenu { get; internal set; }
        public bool Jump { get; internal set; }
        public bool MouseClick { get; internal set; }
        public bool CamArrowUp { get; internal set; }
        public bool CamArrowDown { get; internal set; }
        public bool CamArrowLeft { get; internal set; }
        public bool CamArrowRight { get; internal set; }
        public bool CamGrab { get; internal set; }
        public float XMove { get; internal set; }
        public Vector3 MousePosition { get; internal set; }

        internal void ClearData ()
        {
            this.XMove = 0.0f;
            this.Jump = false;
            this.CamGrab = false;
            this.PauseMenu = false;
            this.MouseClick = false;
            this.CamArrowUp = false;
            this.CamArrowDown = false;
            this.CamArrowLeft = false;
            this.CamArrowRight = false;
            this.CameraZoomIn = false;
            this.CameraZoomOut = false;
            this.CameraZoomOutHold = false;
            this.MousePosition = Vector3.zero;
        }
    }
}