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
        
        public void Execute (InputData data)
        {
            var xMove = Input.GetAxisRaw ("Horizontal");

            data.XMove += xMove;

            data.Jump |= Input.GetKeyDown(_kcJump);
            data.CamArrowUp |= Input.GetKeyDown(_kcCamUp);
            data.CamArrowDown |= Input.GetKeyDown(_kcCamDown);
            data.CamArrowLeft |= Input.GetKeyDown(_kcCamLeft);
            data.CamArrowRight |= Input.GetKeyDown(_kcCamRight);
        }
    }
}