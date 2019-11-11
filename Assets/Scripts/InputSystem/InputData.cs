namespace GhAyoub.InputSystem
{
    public class InputData
    {
        public bool Jump { get; internal set; }
        public bool CamArrowUp { get; internal set; }
        public bool CamArrowDown { get; internal set; }
        public bool CamArrowLeft { get; internal set; }
        public bool CamArrowRight { get; internal set; }
        public float XMove { get; internal set; }

        internal void ClearData ()
        {
            this.XMove = 0.0f;
            this.Jump = false;
            this.CamArrowUp = false;
            this.CamArrowDown = false;
            this.CamArrowLeft = false;
            this.CamArrowRight = false;
        }
    }
}