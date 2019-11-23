using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BatteryCollectable : ICollectable
{
    public override void Collect ()
    {
        base.Collect ();
        Debug.Log ("Collect Battery");
    }

    public override void Animate ()
    {
        Debug.Log ("Animate Battery");

        base.Animate ();
    }
}