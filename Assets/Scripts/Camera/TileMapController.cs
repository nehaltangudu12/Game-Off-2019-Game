using UnityEngine.Tilemaps;
using UnityEngine;

public class TileMapController : Singleton<TileMapController>
{
    private Grid _tilesGrid;
    public Grid TilesGrid => _tilesGrid;

    private TilemapRenderer _tileMapRend;
    public TilemapRenderer TileMapRend => _tileMapRend;

    void Start ()
    {
        TryGetComponent (out _tileMapRend);
        transform.parent.TryGetComponent (out _tilesGrid);
    }
}