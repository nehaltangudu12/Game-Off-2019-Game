using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Battery : MonoBehaviour
{
    [SerializeField] private float UpdateSpeed = 1.5f;
    [SerializeField] private float SubtractByMovement = 0.05f;
    [SerializeField] private Image BatteryTube = null;
    [SerializeField] private GameObject ContentContainer = null;
    [SerializeField] private Transform NormalTransHolder = null;
    [SerializeField] private Transform VirtualTransHolder = null;

    private float _cachedFillAmount;
    private Tween _batteryTween = null;
    private CameraController _camInstance;

    public bool IsBatteryFull => BatteryTube.fillAmount == 1f;
    public bool IsBatteryEmpty => BatteryTube.fillAmount == 0f;

    public void FillAmount (float amount) => BatteryTube.fillAmount += amount;

    void Start ()
    {
        _camInstance = CameraController.Instance;

        _camInstance.BatteryDischarged.AddEvent (UpdateAmount, this);
        _camInstance.IsZoomedOut.AddEvent (UpdateStatus, this);

        DOTween.defaultAutoKill = false;
    }

    private void Update ()
    {
        BatteryChecker ();
    }

    internal void CacheBatteryAmount ()
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

    void UpdateAmount ()
    {
        _batteryTween?.Pause ();

        var targetAmount = BatteryTube.fillAmount - SubtractByMovement;
        
        _batteryTween = BatteryTube.DOFillAmount (targetAmount, 0.1f * Time.unscaledDeltaTime);
    }

    void BatteryChecker ()
    {
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