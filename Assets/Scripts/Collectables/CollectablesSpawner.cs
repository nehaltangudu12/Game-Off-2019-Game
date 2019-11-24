using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesSpawner : MonoBehaviour
{
    [SerializeField] private BatteryCollectable BCollectable;
    [SerializeField] private Transform[] SpawnPoints;

    private List<ICollectable> _collectables = new List<ICollectable> ();

    void Start ()
    {
        SpawnCollectables ();
    }

    void SpawnCollectables ()
    {
        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            var collectable = Instantiate (BCollectable, SpawnPoints[i].position, Quaternion.identity);
            collectable.transform.SetParent (this.transform);
            collectable.Init (this);
            _collectables.Add (collectable);
        }
    }

    internal void Collect (ICollectable collectable)
    {
        ConsumeCollectable (collectable);
    }

    void ConsumeCollectable (ICollectable collectable)
    {
        var index = _collectables.IndexOf (collectable);

        if (index != -1)
        {
            _collectables.RemoveAt (index);
            Destroy (collectable.gameObject);
        }
    }
}