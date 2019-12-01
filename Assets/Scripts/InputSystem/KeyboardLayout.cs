using UnityEngine;

namespace GhAyoub.InputSystem
{
    public class KeyboardLayout : ILayout
    {
        private KeyCode _kcJump = KeyCode.Space;
        private KeyCode _kcCamLeft = KeyCode.LeftArrow;
        private KeyCode _kcCamRight = KeyCode.RightArrow;
        private KeyCode _kcCamUp = KeyCode.UpArrow;
        private KeyCode _kcCamDown = KeyCode.DownArrow;
        private KeyCode _kcCamZoom = KeyCode.C;
        private KeyCode _kcPauseMenu = KeyCode.Escape;

        public void Execute (InputData data)
        {
            var xMove = Input.GetAxisRaw ("Horizontal");

            data.XMove += xMove;

            data.Jump |= Input.GetKeyDown (_kcJump);
            data.PauseMenu |= Input.GetKeyDown (_kcPauseMenu);
            data.CamArrowUp |= Input.GetKeyDown (_kcCamUp);
            data.CamArrowDown |= Input.GetKeyDown (_kcCamDown);
            data.CamArrowLeft |= Input.GetKeyDown (_kcCamLeft);
            data.CamArrowRight |= Input.GetKeyDown (_kcCamRight);
            data.CameraZoomIn |= Input.GetKeyUp (_kcCamZoom);
            data.CameraZoomOut |= Input.GetKeyDown (_kcCamZoom);
            data.CameraZoomOutHold |= Input.GetKey (_kcCamZoom);
        }
    }
}