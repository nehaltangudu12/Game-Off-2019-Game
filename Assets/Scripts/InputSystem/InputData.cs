namespace GhAyoub.InputSystem
{
    public class InputData
    {
        public bool Jump { get; internal set; }
        public float XMove { get; internal set; }

        internal void ClearData ()
        {
            this.XMove = 0.0f;
            this.Jump = false;
        }
    }
}