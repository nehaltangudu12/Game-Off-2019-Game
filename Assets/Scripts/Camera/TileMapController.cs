using UnityEngine.Tilemaps;
using UnityEngine;

public class TileMapController : SingletonMB<TileMapController>
{
    private Grid _tilesGrid;
    public Grid TilesGrid => _tilesGrid;

    private TilemapRenderer _tileMapRend;
    public TilemapRenderer TileMapRend => _tileMapRend;

    public void Init ()
    {
        TryGetComponent (out _tileMapRend);
        transform.parent.TryGetComponent (out _tilesGrid);
    }
}