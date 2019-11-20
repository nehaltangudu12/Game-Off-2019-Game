using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraEffects : MonoBehaviour
{
    [SerializeField] private PostProcessVolume PostVolume;

    private LensDistortion _currentLensDistortion = null;

    void Start ()
    {
        PostVolume.profile.TryGetSettings (out _currentLensDistortion);
    }

    public void LensDistortionStatus (bool enable)
    {
        _currentLensDistortion.active = enable; 
    }
}