using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alternate_PlayerController : MonoBehaviour
{
    public float speed = 10;
    public float jumpHeight;
    public Rigidbody2D rb;
    bool grounded = true;
    private Vector3 startPosition;
    private Vector2 velocitybeforefrozen;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ground")
        {
            grounded = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "ground")
        {
            grounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "ground")
        {
            grounded = false;
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(new Vector3(speed, 0, 0));
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(new Vector3(-speed, 0, 0));
        }
        if (Input.GetKeyDown(KeyCode.W) && grounded == true)
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(new Vector3(1.0f,0.0f,0.0f))))
            {
                rb.AddForce(new Vector3(-(jumpHeight*10), (jumpHeight * 10), 0));
            }
            else if (Physics.Raycast(transform.position, transform.TransformDirection(new Vector3(-1.0f, 0.0f, 0.0f))))
            {
                rb.AddForce(new Vector3(jumpHeight * 10, (jumpHeight * 10), 0));
            }
            else
            {
                rb.AddForce(new Vector3(0, (jumpHeight * 10), 0));
            }
        } 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            velocitybeforefrozen = rb.GetPointVelocity(rb.transform.position);
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.AddForce(velocitybeforefrozen);
        }
    }
}
