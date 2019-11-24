using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffects : MonoBehaviour
{
    [SerializeField] private SpriteGlowEffect CharacterGlow = null;

    internal void PickUpBattery ()
    {
        CharacterGlow.FlashGlow ();
    }
}