using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public float speed = 10;
    public float jumpHeight;
    public Rigidbody2D rb;
    private Vector3 startPosition;
    
    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.None;
        }

        if (Input.GetKey(KeyCode.D)) 
        {
            rb.AddForce(new Vector3(speed, 0, 0));
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(new Vector3(-speed, 0,0));
        }
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(new Vector3(0, jumpHeight,0));
        }
    }
}
