using UnityEngine;

namespace GhAyoub.InputSystem
{
    public class PlayerInput : SingletonMB<PlayerInput>
    {
        public InputData Data { get; private set; } = new InputData ();

        private MouseLayout _mouse = new MouseLayout ();
        private KeyboardLayout _keyboard = new KeyboardLayout ();

        void Update ()
        {
            Data.ClearData ();

            _mouse.Execute (Data);
            _keyboard.Execute (Data);
        }
    }
}