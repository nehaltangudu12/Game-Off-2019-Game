using UnityEngine;

namespace GhAyoub.InputSystem
{
    public class PlayerInput : Singleton<PlayerInput>
    {
        public InputData Data { get; private set; } = new InputData ();

        private KeyboardLayout _keyboard = new KeyboardLayout ();

        void Update ()
        {
            Data.ClearData();

            _keyboard.Execute(Data);
        }
    }
}