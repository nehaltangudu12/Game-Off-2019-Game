using UnityEngine;

namespace GhAyoub.InputSystem
{
    public class MouseLayout : ILayout
    {
        private KeyCode _kcMouseClick= KeyCode.Mouse0;
        private KeyCode _kcCameraGrab = KeyCode.Mouse0;

        public void Execute (InputData data)
        {
            data.CamGrab |= Input.GetKey (_kcCameraGrab);
            data.MouseClick |= Input.GetKeyDown (_kcMouseClick);
            data.MousePosition += Camera.main.ScreenToWorldPoint (Input.mousePosition);
        }
    }
}