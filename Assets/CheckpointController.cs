using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : SingletonMB<CheckpointController>
{
    public float DistanceToApplyNewCheckpoint;
    
    private Vector3 CurrentCheckpoint;

    private void Start()
    {
        CurrentCheckpoint = transform.position;
    }

    public void SetNewCheckpoint(Vector3 newCheckpointPosition)
    {
        if(Vector3.Distance( this.CurrentCheckpoint, newCheckpointPosition) > DistanceToApplyNewCheckpoint)
            this.CurrentCheckpoint = newCheckpointPosition;
    }
    
    public void GoToLastCheckpoint()
    {
        transform.position = CurrentCheckpoint;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "checkpoint")
            CurrentCheckpoint = other.gameObject.transform.position;
    }
}
