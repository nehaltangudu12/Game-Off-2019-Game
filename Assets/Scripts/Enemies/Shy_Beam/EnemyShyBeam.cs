using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShyBeam : MonoBehaviour, IEnemy
{
    [SerializeField] private LaserBeam Laser = null;

    private CameraController _camInstance = null;

    private void Start ()
    {
        _camInstance = CameraController.Instance;

        Laser.Init (SceneController.Instance);
    }

    void Update ()
    {
        var framWorldPos = _camInstance.BoundsWorldPos;

        if (
            this.transform.position.x > framWorldPos.x - 21f &&
            this.transform.position.x < framWorldPos.x + 21f &&
            this.transform.position.y < framWorldPos.y + 12f &&
            this.transform.position.y > framWorldPos.y - 12f)
        {
            Freeze ();
        }
        else
        {
            UnFreeze ();
        }
    }

    public void Init () { }

    public void Freeze ()
    {
        Laser.gameObject.SetActive(false);
    }

    public void UnFreeze ()
    {
        Laser.gameObject.SetActive(true);
    }
}