using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShyBeam : MonoBehaviour, IEnemy
{
    [SerializeField] private LaserBeam Laser = null;

    private void Start ()
    {
        Laser.Init (SceneController.Instance);
    }

    public void Init () { }

    public void Freeze () { }

    public void UnFreeze () { }
}