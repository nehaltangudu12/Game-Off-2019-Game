using System;
using System.Collections;
using System.Collections.Generic;
using GhAyoub.InputSystem;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private bool ShowDebug = false;
    [SerializeField] private float RunSpeed = 600f;
    [SerializeField] private float JumpForce = 400f;
    [SerializeField] private float WallCheckDistRight = 0.2f;
    [SerializeField] private float WallCheckDistLeft = 0.2f;
    [SerializeField] private float GroundCheckRadius = 0.2f;
    [SerializeField] private float WallSlidingSpeedThreshold = 0.2f;
    [SerializeField] private Vector2 WallJumpForce;
    [SerializeField] private LayerMask GroundLayer;
    [SerializeField] private LayerMask WallLayer;
    [SerializeField] private LayerMask BoundsLayer;
    [SerializeField] private Animator PlayerAnim = null;
    [SerializeField] private Transform WallCheckObj = null;
    [SerializeField] private Transform GroundCheckObj = null;
    [SerializeField] private SpriteRenderer PlayerVisual = null;
    [SerializeField] private GUISkin DebugGUISkin = null;
    [SerializeField] private CameraController CamController = null;
    [SerializeField] private CharacterEffects CharEffects = null;
    [SerializeField] private CharacterSound CharSounds = null;
    [SerializeField] private Vector3 startpos;

    private bool _isGrounded = false;
    private bool _isNearWallFromRight = false;
    private bool _isNearWallFromLeft = false;
    private bool _isWallSliding = false;
    private bool _isFacingRight = true;
    private InputData _inputData = null;
    private Rigidbody2D _rdPlayer = null;
    private BoxCollider2D _colliderPlayer = null;
    private Vector3 _storedVelocity = Vector3.zero;
    private GUIStyle _debugStyle = null;


    public CharacterEffects Effects => CharEffects;

    void Awake ()
    {
        TryGetComponent (out _rdPlayer);
        TryGetComponent (out _colliderPlayer);
    }

    void Start ()
    {
        startpos = gameObject.transform.position;
        _inputData = PlayerInput.Instance.Data;
        _debugStyle = DebugGUISkin.label;

        CamController?.Init (this);
    }
    #region  Debug

    private void OnGUI ()
    {
        if (!ShowDebug) return;

        var groundedRect = new Rect (50f, 50f, 250f, 50f);

        GUI.Label (groundedRect, string.Format ("Grounded? => {0}", _isGrounded), _debugStyle);

        var wallRect = new Rect (50f, 110f, 250f, 50f);

        // GUI.Label (wallRect, string.Format ("Near Wall? => {0}", _isNearWall), _debugStyle);

        var wallSlideRect = new Rect (50f, 170f, 250f, 50f);

        GUI.Label (wallSlideRect, string.Format ("Wall Sliding? => {0}", _isWallSliding), _debugStyle);

        var veloRect = new Rect (50f, 230f, 250f, 50f);

        GUI.Label (veloRect, string.Format ("Velocity? => {0}", _rdPlayer.velocity), _debugStyle);
    }

    private void OnDrawGizmos ()
    {
        WallCheckGizmos ();
        GroundCheckGizmos ();
    }

    void GroundCheckGizmos ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere (GroundCheckObj.position, GroundCheckRadius);
    }

    void WallCheckGizmos ()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine (WallCheckObj.position, WallCheckObj.position + new Vector3 (WallCheckDistRight, 0f, 0f));
        Gizmos.DrawLine (WallCheckObj.position, WallCheckObj.position - new Vector3 (WallCheckDistLeft, 0f, 0f));
    }

    #endregion

    private void Update ()
    {
        Animation ();

        FlipIt ();

        WallSideCheck ();

        Movement ();
        
        Jump ();
    }

    private void FixedUpdate ()
    {

        StatesChecker ();
    }

    void Jump ()
    {
        if (_inputData.Jump)
        {
            if (_isGrounded)
            {
                _isGrounded = false;
                _rdPlayer.AddForce (new Vector2 (0, JumpForce));

                // sound
                CharSounds.PlayJumpGround ();
            }
            else if (_isWallSliding)
            {
                _rdPlayer.AddForce (WallJumpForce);

                // sound
                CharSounds.PlayWallJump ();
            }
        }
    }

    void WallSideCheck ()
    {
        _isWallSliding = (_rdPlayer.velocity.y < 0 && (_isNearWallFromLeft || _isNearWallFromRight) && !_isGrounded);
    }

    void Movement ()
    {
        if (_isWallSliding)
        {
            if (_rdPlayer.velocity.y < -WallSlidingSpeedThreshold)
            {
                _rdPlayer.velocity = new Vector2 (_rdPlayer.velocity.x, -WallSlidingSpeedThreshold * Time.deltaTime);
            }
        }
        else
        {
            var targetVelocity = new Vector2 (_inputData.XMove * RunSpeed * Time.deltaTime, _rdPlayer.velocity.y);

            _rdPlayer.velocity = Vector3.SmoothDamp (_rdPlayer.velocity, targetVelocity, ref _storedVelocity, .05f);
        }
    }

    void Animation ()
    {
        if (_isGrounded || _rdPlayer.velocity.y == 0)
        {
            if (Mathf.Abs (_inputData.XMove) > 0)
                PlayerAnim.SetBool ("IsRunning", true);
            else
                PlayerAnim.SetBool ("IsRunning", false);

            PlayerAnim.SetBool ("IsJumping", false);
            PlayerAnim.SetBool ("IsFalling", false);
            PlayerAnim.SetBool ("IsWallSliding", false);
        }
        else if (!_isGrounded)
        {
            if ((_rdPlayer.velocity.y > 0))
            {
                PlayerAnim.SetBool ("IsJumping", true);
                PlayerAnim.SetBool ("IsFalling", false);
                PlayerAnim.SetBool ("IsWallSliding", false);
            }
            else if (_rdPlayer.velocity.y < 0)
            {
                PlayerAnim.SetBool ("IsJumping", false);
                PlayerAnim.SetBool ("IsFalling", !_isWallSliding);
                PlayerAnim.SetBool ("IsWallSliding", _isWallSliding);
            }
        }
    }

    void StatesChecker ()
    {
        _isGrounded = Physics2D.OverlapCircle (GroundCheckObj.position, GroundCheckRadius, GroundLayer);

        _isNearWallFromRight = Physics2D.Raycast (WallCheckObj.position, transform.right, WallCheckDistRight, WallLayer | BoundsLayer);

        _isNearWallFromLeft = Physics2D.Raycast (WallCheckObj.position, -transform.right, WallCheckDistLeft, WallLayer | BoundsLayer);
    }

    void FlipIt ()
    {
        if (!_isWallSliding)
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
        else // if sliding
        {
            if (_isNearWallFromLeft)
            {
                if (!_isFacingRight)
                {
                    transform.localScale = new Vector3 (1.5f, 1.5f, 1.5f);
                }
            }
            else if (_isNearWallFromRight)
            {
                if (_isFacingRight)
                {
                    transform.localScale = new Vector3 (-1.5f, 1.5f, 1.5f);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "enemy")
        {
            transform.position = startpos;
        }

        if (collision.tag == "checkpoint")
        {
            startpos = collision.transform.position;
        }
    }
}