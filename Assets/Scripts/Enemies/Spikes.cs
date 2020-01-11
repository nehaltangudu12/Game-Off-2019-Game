using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private SceneController _sceneControl;

    void Start ()
    {
        _sceneControl = SceneController.Instance;
    }

    /*
    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.GetComponent<CharacterController> () != null)
        {
            _sceneControl.ResetCurrentScene ();
        }
    }
    */
}