using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerConrtoller : MonoBehaviour
{
    public float Speed;
    
    private Vector3 _direction { get; set; }

    private void Start()
    {
        _direction = Vector2.right;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += _direction * Speed * Time.deltaTime;
    }

    void Flip()
    {
        _direction = _direction * -1;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        Flip();
    }
}
