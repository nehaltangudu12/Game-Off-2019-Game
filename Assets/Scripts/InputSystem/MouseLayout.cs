using UnityEngine;

namespace GhAyoub.InputSystem
{
    public class MouseLayout : ILayout
    {
        private KeyCode _kcCameraGrab = KeyCode.Mouse0;

        public void Execute (InputData data)
        {
            data.CamGrab |= Input.GetKey (_kcCameraGrab);
            data.MousePosition += Camera.main.ScreenToWorldPoint (Input.mousePosition);
        }
    }
}