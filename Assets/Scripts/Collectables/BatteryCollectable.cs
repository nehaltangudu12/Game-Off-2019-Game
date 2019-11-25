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

    public override void Collect (GameObject target)
    {
        if (_uiController.BatteryInstance.IsBatteryFull) return;

        base.Collect (target);

        _uiController.FillBattery (.2f);
        target.TryGetComponent (out CharacterEffects effects);
        effects.PickUpBattery ();

        Debug.Log ("Collect Battery");
    }

    public override void Animate ()
    {
        Debug.Log ("Animate Battery");

        base.Animate ();
    }
}