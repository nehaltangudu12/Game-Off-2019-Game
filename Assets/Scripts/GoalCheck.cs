using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalCheck : MonoBehaviour
{
    private SceneController _sceneInstance;

    void Start ()
    {
        _sceneInstance = SceneController.Instance;
    }

    private void OnTriggerEnter2D (Collider2D collider)
    {
        if (collider.TryGetComponent (out CharacterController player))
        {
            var nextSceneId = SceneManager.GetActiveScene ().buildIndex + 1;
            if (nextSceneId >= SceneManager.sceneCountInBuildSettings)
            {
                _sceneInstance.LoadSceneAsync (0);
            }
            else
            {
                _sceneInstance.LoadSceneAsync (nextSceneId);
            }
        }
    }
}