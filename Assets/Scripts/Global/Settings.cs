using System;

namespace UnityEngine
{
    public class Settings
    {
        public static float PixelsInGridX {
            get
            {
                Debug.LogWarning("Please delete this property and all calls right after Level Editor is completed.");
                return 1.26f;
            }
        }
        
        public static float PixelsInGridY {
            get
            {
                Debug.LogWarning("Please delete this property and all calls right after Level Editor is completed.");
                return 0.72f;
            }
        }
    }
}