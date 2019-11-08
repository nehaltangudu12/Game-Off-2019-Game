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

            if (Input.GetKey(KeyCode.RightArrow))
            {
                rb.AddForce(new Vector3(speed, 0, 0));
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rb.AddForce(new Vector3(-speed, 0, 0));
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) && grounded == true)
            {
                rb.AddForce(new Vector3(0, (jumpHeight * 10), 0));
            }
     
    }
}
