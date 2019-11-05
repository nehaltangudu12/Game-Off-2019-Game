using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public Camera cam;
    private string heightorwidth;

    private void Start()
    {
        heightorwidth = "Height";
    }

    void FixedUpdate()
    {
        // Right click to change whether you want to change height or width.
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (heightorwidth == "Height")
            {
                heightorwidth = "Width";
            }
            else
            {
                heightorwidth = "Height";
            }
        }

        // Press space and move mouse to move screen. Also, scroll the mouse wheel when pressing square to edit height/width.
        if (Input.GetKey(KeyCode.Space))
        {
            rb.MovePosition(cam.ScreenToWorldPoint(Input.mousePosition));

            if (heightorwidth == "Height")
            {
                transform.localScale += new Vector3(0, Input.mouseScrollDelta.y, 0);
            }
            else
            {
                transform.localScale += new Vector3(Input.mouseScrollDelta.y, 0, 0);
            }
        }

    }
}
