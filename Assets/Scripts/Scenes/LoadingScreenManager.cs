using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenManager : MonoBehaviour
{
    [SerializeField]
    Transform Circle;
    public float SpinSpeed;

    private SceneController _sceneInstance;

    private void Start()
    {
        _sceneInstance = SceneController.Instance;
        var sceneNumber = PlayerPrefs.GetInt("scene number");
        _sceneInstance.LoadSceneAsync(sceneNumber);
    }

    void Update()
    {
        Vector3 vector = Circle.eulerAngles;
        vector.z -= Time.deltaTime * SpinSpeed;
        Circle.eulerAngles = vector;
    }
}
