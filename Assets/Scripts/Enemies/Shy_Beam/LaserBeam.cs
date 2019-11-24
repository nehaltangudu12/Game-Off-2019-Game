using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [SerializeField] private float LaserDistance = 0.5f;
    [SerializeField] private Transform LaserStartEffect = null;
    [SerializeField] private Transform LaserEndEffect = null;
    [SerializeField, Tooltip ("0: right, 1: left, 2: up, 3: down")] private byte LaserDirection = 0;

    private LineRenderer _lineR;
    private SceneController _sceneControl;

    private void Awake ()
    {
        TryGetComponent (out _lineR);
    }

    internal void Init (SceneController sceneControl)
    {
        _sceneControl = sceneControl;
    }

    private void FixedUpdate ()
    {
        RaycastHit2D hittedObj = Physics2D.Raycast (transform.position, DirectionExtractor (), LaserDistance, 1 << 9 | 1 << 10 | 1 << 11);

        if (hittedObj.collider != null)
        {
            _lineR.SetPosition (0, transform.position);
            _lineR.SetPosition (1, hittedObj.point);

            LaserEndEffect.gameObject.SetActive (true);
            LaserStartEffect.gameObject.SetActive (true);

            LaserEndEffect.position = hittedObj.point;

            if (hittedObj.collider.CompareTag ("Player")) // reset level
            {
                _sceneControl.ResetCurrentScene ();
            }
        }
        else
        {
            LaserEndEffect.gameObject.SetActive (false);
            LaserStartEffect.gameObject.SetActive (false);
        }
    }

    Vector2 DirectionExtractor ()
    {
        switch (LaserDirection)
        {
            case 0:

                return Vector2.right;

            case 1:
                return Vector2.left;

            case 2:
                return Vector2.up;

            case 3:
                return Vector2.down;
        }

        return Vector2.right;
    }
}