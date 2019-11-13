using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public Collider2D Vision;

    private Collider2D _playerCollider;
    
    
    // Start is called before the first frame update
    void Start()
    {
        var player = GameObject.FindWithTag("Player");
        
        if(player == null)
            Debug.LogError("Can't find game object with 'Player' tag!");
        
        _playerCollider = player.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        IsPlayerVisible();
    }

    private bool IsPlayerVisible()
    {
        if(Vision.IsTouching(_playerCollider))
            Debug.Log("Touching");

        return true;
    }
}
