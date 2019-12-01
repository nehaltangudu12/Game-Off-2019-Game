using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : SingletonMB<UIController>
{
    [SerializeField] private Battery Battery = null;

    public Battery BatteryInstance => Battery;

    internal void Init () { }

    public void FillBattery (float amount)
    {
        Battery.FillAmount (amount);
        Battery.CacheBatteryAmount ();
    }
}