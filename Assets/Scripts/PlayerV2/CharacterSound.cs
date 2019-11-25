using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSound : MonoBehaviour
{
    [SerializeField] private AudioClip SoundWallJump = null;
    [SerializeField] private AudioClip SoundJumpGround = null;

    private AudioController _audioControl;

    private void Start ()
    {
        _audioControl = AudioController.Instance;
    }

    internal void PlayWallJump ()
    {
        _audioControl.PlaySfx (SoundWallJump, 1);
    }

    internal void PlayJumpGround ()
    {
        _audioControl.PlaySfx (SoundJumpGround, 1);
    }
}