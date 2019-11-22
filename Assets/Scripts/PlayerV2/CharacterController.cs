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
    [SerializeField] private GUISkin DebugGUISkin = null;

    private bool _grounded = false;
    private bool _isFacingRight = true;
    private InputData _inputData = null;
    private Rigidbody2D _rdPlayer = null;
    private BoxCollider2D _colliderPlayer = null;
    private Vector3 _storedVelocity = Vector3.zero;
    private GUIStyle _debugStyle = null;

    void Awake ()
    {
        TryGetComponent (out _rdPlayer);
        TryGetComponent (out _colliderPlayer);
    }

    void Start ()
    {
        _inputData = PlayerInput.Instance.Data;
        _debugStyle = DebugGUISkin.label;

        CamController.Init (this);
    }
    #region  Debug

    private void OnGUI ()
    {
        var debgRect = new Rect (50f, 50f, 250f, 50f);

        GUI.Label (debgRect, string.Format ("Grounded? => {0}", _grounded), _debugStyle);

        var veloRect = new Rect (50f, 110f, 250f, 50f);

        GUI.Label (veloRect, string.Format ("Velocity? => {0}", _rdPlayer.velocity), _debugStyle);
    }

    private void OnDrawGizmos ()
    {
        GroundCheckGizmos ();
    }

    void GroundCheckGizmos ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere (GroundCheckObj.position, GroundCheckRadius);
    }
    #endregion

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
        if (_grounded || _rdPlayer.velocity.y == 0)
        {
            PlayerAnim.SetBool ("IsJumping", false);
            PlayerAnim.SetBool ("IsFalling", false);
        }

        if (!_grounded)
        {
            if ((_rdPlayer.velocity.y > 0))
            {
                PlayerAnim.SetBool ("IsJumping", true);
                PlayerAnim.SetBool ("IsFalling", false);
            }
            else if (_rdPlayer.velocity.y < 0)
            {
                PlayerAnim.SetBool ("IsJumping", false);
                PlayerAnim.SetBool ("IsFalling", true);
            }
        }
        else
        {
            if (Mathf.Abs (_inputData.XMove) > 0)
            {
                PlayerAnim.SetBool ("IsRunning", true);
            }
            else
                PlayerAnim.SetBool ("IsRunning", false);
        }
    }

    void GroundCheck ()
    {
        _grounded = false;

        var colliders = Physics2D.OverlapCircleAll (GroundCheckObj.position, GroundCheckRadius, GroundLayer);

        for (int i = 0; i < colliders.Length; i++)
            if (colliders[i].gameObject != gameObject)
                _grounded = true;
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

        transform.localScale = new Vector3 (_isFacingRight ? 1.5f : -1.5f, 1.5f, 1.5f);
    }
}