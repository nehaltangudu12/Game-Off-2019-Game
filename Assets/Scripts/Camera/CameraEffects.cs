using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraEffects : MonoBehaviour
{
    [SerializeField] private PostProcessVolume PostVolume;

    private LensDistortion _currentLensDistortion = null;
    private DepthOfField _currentDepthOfField = null;

    void Start ()
    {
        PostVolume.profile.TryGetSettings (out _currentDepthOfField);
        PostVolume.profile.TryGetSettings (out _currentLensDistortion);

        DepthOfFieldAnim ();
    }

    void DepthOfFieldAnim ()
    {
        DOTween.To (() => { return _currentDepthOfField.aperture.value; }, val => { _currentDepthOfField.aperture.value = val; }, 31f, 3);
    }

    public void LensDistortionStatus (bool enable, int intensity = 15)
    {
        _currentLensDistortion.intensity.value = intensity;
        _currentLensDistortion.active = enable;
    }
}