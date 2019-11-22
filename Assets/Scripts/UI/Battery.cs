using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Battery : MonoBehaviour
{
    [SerializeField] private float UpdateSpeed = 1.5f;
    [SerializeField] private Image BatteryTube = null;
    [SerializeField] private GameObject ContentContainer = null;
    [SerializeField] private Transform NormalTransHolder = null;
    [SerializeField] private Transform VirtualTransHolder = null;

    private float _cachedFillAmount;
    private Tween _batteryTween = null;
    private CameraController _camInstance;

    void Start ()
    {
        _camInstance = CameraController.Instance;

        _camInstance.BatteryStatus.AddEvent (UpdateBattery, this);
        _camInstance.IsZoomedOut.AddEvent (UpdateStatus, this);

        DOTween.defaultAutoKill = false;
    }

    private void Update ()
    {
        BatteryChecker ();
    }

    internal void CacheBatteryAmount()
    {
        _cachedFillAmount = BatteryTube.fillAmount;
    }

    private void UpdateStatus (bool zoomOut)
    {
        if (zoomOut)
        {
            ContentContainer.transform.position = NormalTransHolder.position;
        }
        else
        {
            ContentContainer.transform.position = VirtualTransHolder.position;
        }
    }

    private void UpdateBattery (bool emptyIt)
    {
        _batteryTween?.Pause ();

        BatteryTube.fillAmount = _cachedFillAmount;
        
        if (emptyIt)
        {
            // Debug.Log ("Fill amount when emptying : " + BatteryTube.fillAmount);
            _batteryTween = BatteryTube.DOFillAmount (0, 4f * Time.unscaledDeltaTime).SetDelay (UpdateSpeed * Time.unscaledDeltaTime);
        }
        else
        {
            // Debug.Log ("Fill amount when filling : " + BatteryTube.fillAmount);
            _batteryTween = BatteryTube.DOFillAmount (1, 4f);
        }
    }

    void BatteryChecker ()
    {
        // if (CameraBatteryTube.fillAmount <= 0.15f)
        // {
        //     if (_isZoomedOut) CameraZoom (true);
        // }

        if (BatteryTube.fillAmount < 0.3f)
        {
            BatteryTube.color = Color.red;
        }
        else if (BatteryTube.fillAmount < 0.7f)
        {
            BatteryTube.color = Color.yellow;
        }
        else
        {
            BatteryTube.color = Color.green;
        }
    }
}