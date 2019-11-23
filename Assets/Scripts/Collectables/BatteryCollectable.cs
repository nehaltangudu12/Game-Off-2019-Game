using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BatteryCollectable : ICollectable
{
    private UIController _uiController;

    private void Start ()
    {
        _uiController = UIController.Instance;
    }

    public override void Collect ()
    {
        base.Collect ();
        _uiController.FillBattery (.2f);
        Debug.Log ("Collect Battery");
    }

    public override void Animate ()
    {
        Debug.Log ("Animate Battery");

        base.Animate ();
    }
}