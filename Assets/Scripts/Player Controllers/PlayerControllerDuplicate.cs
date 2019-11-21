using System.Collections;
using System.Collections.Generic;
using GhAyoub.InputSystem;
using UnityEngine;

public class PlayerControllerDuplicate : MonoBehaviour
{
    [SerializeField, Tooltip ("Max speed, in units per second, that the character moves.")]
    float speed = 9;

    [SerializeField, Tooltip ("Acceleration while grounded.")]
    float walkAcceleration = 75;

    [SerializeField, Tooltip ("Acceleration while in the air.")]
    float airAcceleration = 30;

    [SerializeField, Tooltip ("Deceleration applied when character is grounded and not attempting to move.")]
    float groundDeceleration = 70;

    [SerializeField, Tooltip ("Max height the character will jump regardless of gravity")]
    float jumpHeight = 4;

    [SerializeField, Tooltip ("Heightboost gained by walljumping")]
    float walljumpboost = 5;

    [SerializeField, Tooltip ("Distance for Jump Buffering")]
    float jbdistance = 5;

    [SerializeField] private float GravityMultiplier = 1f;

    [SerializeField] private Animator PlayerAnimator = null;
    [SerializeField] private SpriteRenderer PlayerSprite = null;

    public CameraController CameraController;

    public BoxCollider2D boxCollider;

    public Vector2 velocity;

    public bool grounded;

    public bool walljump;

    public bool jumped;

    public Collider2D lastwall;

    public bool buffering;

    public bool bufferedjump;

    private InputData _inputData;

    private void Awake ()
    {
        boxCollider = transform.GetComponent<BoxCollider2D> ();
    }

    void Start ()
    {
        _inputData = PlayerInput.Instance.Data;

        //CameraController.Init (this); //injection
    }

    void Update ()
    {
        JumpingLogic ();

        // simulating physics for the player
        if (!grounded)
        {
            velocity.y += Physics2D.gravity.y * GravityMultiplier * Time.deltaTime;
        }

        MovementLogic ();

        VisualLogic ();

        // clears grounded each frame
        grounded = false;

        CollisionLogic ();

    }

    private void JumpingLogic ()
    {
        // buffered jump
        if (buffering)
        {
            if (_inputData.Jump)
            {
                bufferedjump = true;
            }
        }

        if (bufferedjump && grounded)
        {
            Debug.Log ("buffer jump");
            velocity.y = Mathf.Sqrt (2 * jumpHeight * Mathf.Abs (Physics2D.gravity.y));
            bufferedjump = false;
        }
        // normal and wall jumping
        if (!bufferedjump)
        {
            // Only allows the Player to Jump if he isnt in the air
            if ((grounded || walljump))
            {
                if (_inputData.Jump)
                {
                    velocity.y = 0;
                    // if the player is "allowed" to walljump (walljump == true + !jumped == false ) and he is not on the ground he performs a wall jump
                    if (walljump && !jumped && !grounded)
                    {
                        Debug.Log ("wall jump");
                        // moves in the opposite x directione -> bouncing of the wall
                        velocity.x = -velocity.x;
                        // gaining height with the walljump
                        velocity.y += walljumpboost;
                        jumped = true;
                    }
                    else if (grounded)
                    {
                        Debug.Log ("normal jump");
                        velocity.y = Mathf.Sqrt (2 * jumpHeight * Mathf.Abs (Physics2D.gravity.y));
                    }
                }
                walljump = false;
            }
        }
    }
    private void MovementLogic ()
    {
        // bounds
        var camPos = CameraController.transform.position;

        var minX = camPos.x - 21f;
        var maxX = camPos.x + 21f;

        var bounds = new Vector2 (minX, maxX);

        transform.Translate (velocity * Time.deltaTime);

        // Clamp
        var targetPos = new Vector2 (Mathf.Clamp (transform.position.x, bounds.x, bounds.y), transform.position.y);

        if (transform.position.x < bounds.x || transform.position.x > bounds.y)
        {
            transform.position = targetPos;
        }

        // Changes the acc-/deceleration according to the players current state of being grounded or not
        float acceleration = grounded ? walkAcceleration : airAcceleration;
        float deceleration = grounded ? groundDeceleration : 0;

        // Speeds up while getting input and slows down when not getting input
        if (_inputData.XMove != 0)
        {
            velocity.x = Mathf.MoveTowards (velocity.x, speed * _inputData.XMove, acceleration * Time.deltaTime);
        }
        else
        {
            velocity.x = Mathf.MoveTowards (velocity.x, 0, deceleration * Time.deltaTime);
        }

        if (velocity.x != 0)
        {
            _isLastMovementLeft = velocity.x < 0;
        }
    }

    private bool _isLastMovementLeft = false;
    private void VisualLogic ()
    {
        PlayerAnimator.SetFloat ("Velocity", velocity.x);
        PlayerSprite.flipX = _isLastMovementLeft;
    }

    private void CollisionLogic ()
    {

        // Gets all Collissions in the own 2dCollider
        Collider2D[] hits = Physics2D.OverlapBoxAll (transform.position, boxCollider.size, 0);

        // Loops through all Collisions and pushed the player out of other colliders by the minimum Distance required
        foreach (Collider2D hit in hits)
        {
            // skips own collider 
            if (hit == boxCollider)
                continue;

            ColliderDistance2D colliderDistance = hit.Distance (boxCollider);

            // checks if colliders are still overlapping (could have changed cause an collider earlier in the array already caused the player to be pushed out)
            // and if so moves the player away by the min distance also checks if the player is colliding with an "ground" object
            if (colliderDistance.isOverlapped)
            {
                transform.Translate (colliderDistance.pointA - colliderDistance.pointB);
                // if the player is touching a object with the ground tag its "grounded"
                if (hit.CompareTag ("ground"))
                {
                    grounded = true;

                    jumped = false;
                }
                if (hit.CompareTag ("jumpwall") && hit != lastwall && jumped)
                {
                    jumped = false;
                    walljump = true;
                }
                // Allows the player to walljump
                if (hit.CompareTag ("jumpwall") && !walljump)
                {
                    if (!grounded)
                    {
                        walljump = true;
                        lastwall = hit;
                    }
                }
            }
        }
        buffering = false;
        if (!grounded)
        {
            Collider2D[] jbhits = Physics2D.OverlapBoxAll (transform.position, new Vector2 (boxCollider.size.x + jbdistance, boxCollider.size.y + jbdistance), 0);

            foreach (Collider2D hit in jbhits)
            {
                if (!buffering && hit.CompareTag ("ground") && velocity.y < 0)
                {
                    buffering = true;
                }
            }
        }
    }

    private void FixedUpdate ()
    {

    }
}