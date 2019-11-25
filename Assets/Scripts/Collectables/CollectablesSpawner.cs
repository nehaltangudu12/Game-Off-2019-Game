using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class CollectablesSpawner : MonoBehaviour
{
    [SerializeField] private float ReSpawnDelay = 1.5f;
    [SerializeField] private BatteryCollectable BCollectable;
    [SerializeField] private SpawnData[] SpawnPoints;

    private UIController _uiControl;
    private List<ICollectable> _collectables = new List<ICollectable> ();

    void Start ()
    {
        _uiControl = UIController.Instance;

        SpawnCollectables ();
    }

    void SpawnCollectables ()
    {
        for (int i = 0; i < SpawnPoints.Length; i++)
            SpawnCollectable (i);
    }

    async Task AsyncSpawn (int id)
    {
        var delayInt = (int) ReSpawnDelay * 1000;
        await Task.Delay (delayInt);

        SpawnCollectable (id);
    }

    void SpawnCollectable (int id)
    {
        var spawnData = SpawnPoints[id];
        var collectable = Instantiate (BCollectable, spawnData.Point.position, Quaternion.identity);
        collectable.transform.SetParent (this.transform);
        collectable.Init (this);

        spawnData.Collectable = collectable;
        _collectables.Add (collectable);
    }

    internal void Collect (ICollectable collectable)
    {
        ConsumeCollectable (collectable);
    }

    void ConsumeCollectable (ICollectable collectable)
    {
        var index = _collectables.IndexOf (collectable);

        var data = SpawnPoints.SingleOrDefault (sp => sp.Collectable == collectable);

        if (index != -1)
        {
            _collectables.RemoveAt (index);
            Destroy (collectable.gameObject);

            _ = AsyncSpawn (data.Id);
        }
    }
}

[System.Serializable]
class SpawnData
{
    public int Id;
    public Transform Point;
    public ICollectable Collectable;
}