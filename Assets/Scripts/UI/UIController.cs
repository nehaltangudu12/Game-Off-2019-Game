using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : Singleton<UIController>
{
    [SerializeField] private Battery Battery = null;

    internal void Init () { }

    public void FillBattery (float amount)
    {
        Battery.FillAmount (amount);
        Battery.CacheBatteryAmount ();
    }
}