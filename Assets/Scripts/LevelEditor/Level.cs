using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
	public string name; // name of the level
	public Vector2 gridDims; // dimensions of the level in grid tiles
	public Vector2 camDims; // pixel dimensions of the Camera View window the level must fit in

	public Tile[] tiles; // array of tiles in the level
	public float tilePixelSize; // calculated using grid size and camera dims
	public Vector2 tilePixelOffset;

	public Level(Vector2 _gridDims, Vector2 _camDims)
	{
		gridDims = _gridDims;
		camDims = _camDims;
		SetTilePixelSize();
		CreateTileGrid();
	}


	public void SetTilePixelSize ()
	{
		float x = camDims.x / (gridDims.x + 2);
		float y = camDims.y / (gridDims.y + 2);

		if (x >= y)
		{
			tilePixelSize = y;
		}
		else
		{
			tilePixelSize = x;
		}

		tilePixelOffset = new Vector2(-tilePixelSize * .5f * (gridDims.x+2), -tilePixelSize * .5f * (gridDims.y+2));
	}

	


	public Tile PointToTile (Vector2 worldPoint)
	{
		Tile foundTile = tiles[0];
		foreach (Tile t in tiles)
		{
			if (worldPoint.x >= t.screenPos.x &&
				worldPoint.x < t.screenPos.x + tilePixelSize &&
				worldPoint.y >= t.screenPos.y &&
				worldPoint.y < t.screenPos.y + tilePixelSize)
			{
				foundTile = t;
			}
		}
		return foundTile;
	}


	/*void BorderToMesh()
	{
		//add the border to the tile mesh
		Vector3[] borderVerts = new Vector3[8]
		{
			boundsTopLeft + new Vector2(-tilePixelSize,tilePixelSize),
			boundsTopRight + new Vector2(tilePixelSize,tilePixelSize),
			boundsBotRight + new Vector2(tilePixelSize,-tilePixelSize),
			boundsBotLeft + new Vector2(-tilePixelSize,-tilePixelSize),
			boundsTopLeft,
			boundsTopRight,
			boundsBotRight,
			boundsBotLeft
		};

		int[] borderTris = new int[24]{0,1,4,1,4,5,1,5,2,5,2,6,6,2,7,2,7,3,7,3,0,0,7,4};

		Mesh mesh = new Mesh();
		borderMesh.mesh = mesh;
		mesh.vertices = borderVerts;
		mesh.triangles = borderTris;
	}*/


	public Mesh TilesToMesh()
	{
		int triPos=0;
		int vertPos=0;
		int currentVerts = 0;
		Vector3[] newVerts = new Vector3[CountVerts()];
		int[] newTris = new int[CountTris()];
		
		foreach (Tile t in tiles)
		{
			if (!t.isEmpty)
			{
				for(int s = 0; s < t.verts.Length; s++)
				{
					Vector3 newVertex = t.verts[s] * tilePixelSize + t.screenPos;
					newVerts[vertPos] = newVertex;
					vertPos++;
				}

				for(int a = 0; a < t.tris.Length; a++)
				{
					int newTri = t.tris[a] + currentVerts;
					newTris[triPos] = newTri;
					triPos++;
				}
				currentVerts += t.verts.Length;
			}
		}

		Mesh mesh = new Mesh();
		mesh.vertices = newVerts;
		mesh.triangles = newTris;
		return mesh;
	}

	int CountVerts()
	{
		int totalVerts = 0;
		foreach (Tile t in tiles)
		{
			if (!t.isEmpty)
			{
				totalVerts+=t.verts.Length;
			}
		}
		return totalVerts;
	}

	int CountTris()
	{
		int totalTris = 0;
		foreach (Tile t in tiles)
		{
			if (!t.isEmpty)
			{
				totalTris+=t.tris.Length;
			}
		}
		return totalTris;
	}


	void CreateTileGrid ()
	{
		int gridSize = ((int)gridDims.x+2) * ((int)gridDims.y + 2);
		
		tiles = new Tile[gridSize];

		int arrayPos = 0;


		for (int r = 0; r < gridDims.y+2; r++)
		{
			for (int c = 0; c < gridDims.x+2; c++)
			{
				
				bool isBorder;
				if (r == 0 || r == gridDims.y+1 || c == 0 || c == gridDims.x+1)
				{
					isBorder = true;
				}
				else
				{
					isBorder = false;
				}
				Vector2 coord = new Vector2(c, r);
				tiles[arrayPos] = new Tile(isBorder, arrayPos, coord);
				tiles[arrayPos].SetScreenPos(tilePixelSize, tilePixelOffset);
				arrayPos++;
			}
		}
	}



}
