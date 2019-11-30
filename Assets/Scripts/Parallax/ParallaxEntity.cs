using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEntity : MonoBehaviour
{
    public float SpeedCoef = 0.2f;

    private Vector3 _lastPos = Vector2.zero;
    private CharacterController _player;

    void Start ()
    {
        _player = FindObjectOfType<CharacterController> ();
        _lastPos = _player.transform.position;
    }

    void Update ()
    {
        transform.position -= new Vector3 (((_lastPos.x - _player.transform.position.x) * SpeedCoef), 0f, 0f);

        _lastPos = _player.transform.position;
    }
}