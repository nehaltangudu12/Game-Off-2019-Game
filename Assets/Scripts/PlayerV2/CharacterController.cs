using System;
using System.Collections;
using System.Collections.Generic;
using GhAyoub.InputSystem;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float RunSpeed = 600f;
    [SerializeField] private float JumpForce = 400f;
    [SerializeField] private float GroundCheckRadius = 0.2f;
    [SerializeField] private LayerMask GroundLayer;
    [SerializeField] private Animator PlayerAnim = null;
    [SerializeField] private Transform GroundCheckObj = null;
    [SerializeField] private SpriteRenderer PlayerVisual = null;
    [SerializeField] private CameraController CamController = null;

    private bool _grounded = false;
    private bool _isFacingRight = true;
    private InputData _inputData = null;
    private Rigidbody2D _rdPlayer = null;
    private BoxCollider2D _colliderPlayer = null;
    private Vector3 _storedVelocity = Vector3.zero;

    void Awake ()
    {
        TryGetComponent (out _rdPlayer);
        TryGetComponent (out _colliderPlayer);
    }

    void Start ()
    {
        _inputData = PlayerInput.Instance.Data;

        CamController.Init (this);
    }

    private void OnDrawGizmos ()
    {
        GroundCheckGizmos ();
    }

    private void Update ()
    {
        Animation ();

        FlipIt ();
    }

    private void FixedUpdate ()
    {
        GroundCheck ();

        Movement ();

        Jump ();
    }

    void Jump ()
    {
        if (_grounded && _inputData.Jump)
        {
            _grounded = false;
            _rdPlayer.AddForce (new Vector2 (0, JumpForce));
        }
    }

    void Movement ()
    {
        var targetVelocity = new Vector2 (_inputData.XMove * RunSpeed * Time.fixedDeltaTime, _rdPlayer.velocity.y);

        _rdPlayer.velocity = Vector3.SmoothDamp (_rdPlayer.velocity, targetVelocity, ref _storedVelocity, .05f);
    }

    void Animation ()
    {
        PlayerAnim.SetFloat ("velocity", _rdPlayer.velocity.x);
        PlayerAnim.SetFloat ("velocityY", _rdPlayer.velocity.y);
    }

    void GroundCheck ()
    {
        _grounded = false;

        var colliders = Physics2D.OverlapCircleAll (GroundCheckObj.position, GroundCheckRadius, GroundLayer);

        for (int i = 0; i < colliders.Length; i++)
            if (colliders[i].gameObject != gameObject)
                _grounded = true;
    }

    void GroundCheckGizmos ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere (GroundCheckObj.position, GroundCheckRadius);
    }

    void FlipIt ()
    {
        if (_inputData.XMove < 0)
        {
            _isFacingRight = false;
        }
        else if (_inputData.XMove > 0)
        {
            _isFacingRight = true;
        }

        transform.localScale = new Vector3 (_isFacingRight ? 1.5f : -1.5f, 1.5f);
    }
}