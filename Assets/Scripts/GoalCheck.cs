using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCheck : MonoBehaviour
{
    private SceneController _sceneInstance;

    void Start ()
    {
        _sceneInstance = SceneController.Instance;
    }

    private void OnTriggerEnter2D (Collider2D collider)
    {
        if (collider.TryGetComponent(out CharacterController player))
        {
            _sceneInstance.LoadSceneAsync (0);
        }
    }
}