using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
    float speed = 9;

    [SerializeField, Tooltip("Acceleration while grounded.")]
    float walkAcceleration = 75;

    [SerializeField, Tooltip("Acceleration while in the air.")]
    float airAcceleration = 30;

    [SerializeField, Tooltip("Deceleration applied when character is grounded and not attempting to move.")]
    float groundDeceleration = 70;

    [SerializeField, Tooltip("Max height the character will jump regardless of gravity")]
    float jumpHeight = 4;

    private BoxCollider2D boxCollider;

    private Vector2 velocity;

    private bool grounded;

    private float horizonzalInput;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }


    void Update()
    {
        // Only allows the Player to Jump if he isnt in the air
        if (grounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y));
            }
        }
        velocity.y += Physics2D.gravity.y * Time.deltaTime;

        horizonzalInput = Input.GetAxisRaw("Horizontal");
        transform.Translate(velocity * Time.deltaTime);

        // Changes the acc-/deceleration according to the players current state of being grounded or not
        float acceleration = grounded ? walkAcceleration : airAcceleration;
        float deceleration = grounded ? groundDeceleration : 0;

        // Speeds up while getting input and slows down when not getting input
        if (horizonzalInput != 0)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, speed * horizonzalInput, acceleration * Time.deltaTime);
        }
        else
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);
        }

        // Gets all Collissions in the own 2dCollider
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);

        // clears grounded each frame
        grounded = false;

        // Loops through all Collisions and pushed the player out of other colliders by the minimum Distance required
        foreach (Collider2D hit in hits)
        {
            // skips own collider 
            if (hit == boxCollider)
                continue;

            ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

            // checks if colliders are still overlapping (could have changed cause an collider earlier in the array already caused the player to be pushed out)
            // and if so moves the player away by the min distance also checks if the player is colliding with an "ground" object
            if (colliderDistance.isOverlapped)
            {
                transform.Translate(colliderDistance.pointA - colliderDistance.pointB);
                // if the player is touching a object with the ground tag its "grounded"
                if (hit.CompareTag("ground"))
                {
                    grounded = true;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        
    }
}
