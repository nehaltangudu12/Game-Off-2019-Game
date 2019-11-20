using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class MainMenuCameraController : MonoBehaviour
{
    [SerializeField] private PostProcessVolume PostVolume;

    private DepthOfField _currentDepthOfField = null;

    void Start ()
    {
        PostVolume.profile.TryGetSettings (out _currentDepthOfField);
    }

    public void DepthOfFieldAnim ()
    {
        DOTween.To (() => { return _currentDepthOfField.aperture.value; }, val => { _currentDepthOfField.aperture.value = val; }, 0.05f, 1.5f);
    }
}