using UnityEngine;

namespace GhAyoub.InputSystem
{
    public class KeyboardLayout : ILayout
    {
        private KeyCode _kcJump = KeyCode.Space;

        public void Execute (InputData data)
        {
            var xMove = Input.GetAxisRaw ("Horizontal");

            data.XMove += xMove;

            data.Jump |= Input.GetKeyDown(_kcJump);
        }
    }
}