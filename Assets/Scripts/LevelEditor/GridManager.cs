using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{/*
	public Vector2 maxDims;
	Vector2[] corners;
	public Color gridColor;
	public Color scrollColor;
	public Color shapeColor;
	Vector2[] topPoints;
	Vector2[] botPoints;
	Vector2[] leftPoints;
	Vector2[] rightPoints;

	Vector2 offset;

	public bool debugTileShapes = true;

	public MeshFilter tileMesh;
	public MeshFilter borderMesh;


	public float tilePixelSize;
	public bool showGrid = true;
	public RectTransform mapBG;
	public RectTransform borderBG;
	bool loading = false;
	Tile[] currentMap;
	Tile scrolledTile;


	Vector2 boundsTopRight;
	Vector2 boundsTopLeft;
	Vector2 boundsBotRight;
	Vector2 boundsBotLeft;



	


	void Start()
	{
		if (!loading)// create a new map if we don't have one to load
		{
			offset = new Vector2(-tilePixelSize * .5f * (maxDims.x+2), -tilePixelSize * .5f * (maxDims.y+2));
			currentMap = NewGrid();
			scrolledTile = currentMap[0];
			mapBG.sizeDelta = maxDims * tilePixelSize;
			borderBG.sizeDelta = (maxDims + new Vector2(2,2)) * tilePixelSize;
			boundsBotLeft = new Vector2(offset.x + tilePixelSize, offset.y + tilePixelSize);
			boundsBotRight = new Vector2(offset.x*-1 - tilePixelSize, offset.y + tilePixelSize);
			boundsTopLeft = new Vector2(offset.x + tilePixelSize, offset.y*-1 - tilePixelSize);
			boundsTopRight = new Vector2(offset.x*-1 - tilePixelSize, offset.y*-1 - tilePixelSize);
			activeShape = Tile.Shape.HalfTri;
			activeBrushIcon = brushIconHalfTri;
			PaletteSwap();
			BorderMesh();
		}
		else
		{
			//load map 
		}
	}


	void BorderMesh()
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
	}


	int CountVerts()
	{
		int totalVerts = 0;
		foreach (Tile t in currentMap)
		{
			if (!t.isEmpty && t.type == Tile.Type.Main)
			{
				totalVerts+=t.points.Length;
			}
		}
		return totalVerts;
	}

	int CountTris()
	{
		int totalTris = 0;
		foreach (Tile t in currentMap)
		{
			if (!t.isEmpty && t.type == Tile.Type.Main)
			{
				totalTris+=t.tris.Length;
			}
		}
		return totalTris;
	}


	void UpdateMesh()
	{
		int triPos=0;
		int vertPos=0;
		int currentVerts = 0;
		Vector3[] newVerts = new Vector3[CountVerts()];
		int[] newTris = new int[CountTris()];
		
		foreach (Tile t in currentMap)
		{
			if (!t.isEmpty && t.type == Tile.Type.Main)
			{
				for(int s = 0; s < t.points.Length; s++)
				{
					Vector3 newVertex = t.points[s] * tilePixelSize + t.screenPosition;
					newVerts[vertPos] = newVertex;
					vertPos++;
				}

				for(int a = 0; a < t.tris.Length; a++)
				{
					int newTri = t.tris[a] + currentVerts;
					newTris[triPos] = newTri;
					triPos++;
				}
				currentVerts += t.points.Length;
			}
		}

		Mesh mesh = new Mesh();
		tileMesh.mesh = mesh;
		mesh.vertices = newVerts;
		mesh.triangles = newTris;
	}


	void Update()
	{
		if (Input.GetKeyDown("f1"))
		{
			PaletteSwap(1);
		}
		if (Input.GetKeyDown("f2"))
		{
			PaletteSwap(2);
		}
		if (Input.GetKeyDown("f3"))
		{
			PaletteSwap(3);
		}
		if (Input.GetKeyDown("f4"))
		{
			PaletteSwap(4);
		}



		UpdateTileBrush();
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		if (mousePos.x >= offset.x + tilePixelSize && mousePos.x <= offset.x*-1 - tilePixelSize && mousePos.y >= offset.y + tilePixelSize && mousePos.y <= offset.y*-1 - tilePixelSize)
		{
			Tile currentTile = WorldToGrid(mousePos, currentMap);
			if (currentTile != scrolledTile && currentTile.type != Tile.Type.Border)
			{
				scrolledTile = currentTile;
			}

			//Debug.DrawLine(scrolledTile.screenPosition, new Vector2(scrolledTile.screenPosition.x + tilePixelSize, scrolledTile.screenPosition.y), scrollColor);
			//Debug.DrawLine(scrolledTile.screenPosition, new Vector2(scrolledTile.screenPosition.x, scrolledTile.screenPosition.y + tilePixelSize), scrollColor);
			//Debug.DrawLine(scrolledTile.screenPosition + new Vector2(tilePixelSize, tilePixelSize), new Vector2(scrolledTile.screenPosition.x + tilePixelSize, scrolledTile.screenPosition.y), scrollColor);
			//Debug.DrawLine(scrolledTile.screenPosition + new Vector2(tilePixelSize, tilePixelSize), new Vector2(scrolledTile.screenPosition.x, scrolledTile.screenPosition.y + tilePixelSize), scrollColor);
		
			if (scrolledTile.type != Tile.Type.Border)
			{
				bool shapeBrush = false;
				int turns = 0;
				bool flipVert = false;
				bool flipHor = false;
				if(Input.GetKey("q"))
				{
					shapeBrush = true;
					turns = 0;
					flipVert = false;
					flipHor = false;
				}
				if(Input.GetKey("w"))
				{
					shapeBrush = true;
					turns = 3;
					flipVert = false;
					flipHor = true;
				}
				if(Input.GetKey("s"))
				{
					shapeBrush = true;
					turns = 2;
					flipVert = true;
					flipHor = true;
				}
				if(Input.GetKey("a"))
				{
					shapeBrush = true;
					turns = 1;
					flipVert = true;
					flipHor = false;
				}
				if (shapeBrush)
				{
					scrolledTile.SetPoints(activeShape, flipVert, flipHor, turns);
					UpdateMesh();
				}

				if (Input.GetKey("e"))
				{
					scrolledTile.SetPoints(Tile.Shape.Full, false, false);
					UpdateMesh();
				}
				if (Input.GetKey("d"))
				{
					scrolledTile.SetPoints(Tile.Shape.Empty, false, false);
					UpdateMesh();
				}
			}

		}

		if (showGrid)
		{
			for (int v = 0; v < topPoints.Length; v++)
			{
				Debug.DrawLine(topPoints[v], botPoints[v], gridColor);
			}
			for (int h = 0; h < leftPoints.Length; h++)
			{
				Debug.DrawLine(leftPoints[h], rightPoints[h], gridColor);
			}
		}


		//Debug.DrawLine(boundsTopLeft, boundsTopRight, scrollColor);
		//Debug.DrawLine(boundsTopLeft, boundsBotLeft, scrollColor);
		//Debug.DrawLine(boundsBotRight, boundsTopRight, scrollColor);
		//Debug.DrawLine(boundsBotRight, boundsBotLeft, scrollColor);

		
		foreach (Tile t in currentMap)
		{
			if(!t.isEmpty && t.type == Tile.Type.Main)
			{
				for (int s = 0; s < t.points.Length; s++)
				{
					if (s!=0)
					{
						Debug.DrawLine(t.points[s] * tilePixelSize + t.screenPosition, t.points[s-1] * tilePixelSize + t.screenPosition, shapeColor);
					}
					else
					{
						Debug.DrawLine(t.points[0] * tilePixelSize + t.screenPosition, t.points[t.points.Length-1] * tilePixelSize + t.screenPosition, shapeColor);
					}
				}
			}
		}
	}



	public Tile WorldToGrid (Vector2 worldPos, Tile[] grid)
	{
		Tile foundTile = scrolledTile;
		foreach (Tile t in grid)
		{
			if (worldPos.x >= t.screenPosition.x &&
				worldPos.x < t.screenPosition.x + tilePixelSize &&
				worldPos.y >= t.screenPosition.y &&
				worldPos.y < t.screenPosition.y + tilePixelSize)
			{
				foundTile = t;
			}
		}
		return foundTile;
	}



	public Tile[] NewGrid ()
	{
		int gridSize = ((int)maxDims.x+2) * ((int)maxDims.y + 2);

		Tile[] tileData = new Tile[gridSize];
		int arrayPos = 0;

		int botPointCount = 0;
		int topPointCount = 0;
		int leftPointCount = 0;
		int rightPointCount = 0;
		botPoints = new Vector2[(int)maxDims.x+1];
		topPoints = new Vector2[(int)maxDims.x+1];
		leftPoints = new Vector2[(int)maxDims.y+1];
		rightPoints = new Vector2[(int)maxDims.y+1];



		for (int r = 0; r < maxDims.y+2; r++)
		{
			for (int c = 0; c < maxDims.x+2; c++)
			{
				tileData[arrayPos] = new Tile();
				Vector2 gridCoord = new Vector2(c, r);
				tileData[arrayPos].coord = gridCoord;
				tileData[arrayPos].screenPosition = new Vector2(gridCoord.x * tilePixelSize + offset.x, gridCoord.y * tilePixelSize + offset.y);
				
				if (r == 0 || r == maxDims.y+1 || c == 0 || c == maxDims.x+1)
				{
					tileData[arrayPos].type = Tile.Type.Border;
					tileData[arrayPos].SetPoints(Tile.Shape.Full, false, false);

					if(r == 0 && botPointCount <= maxDims.x)
					{
						botPoints[botPointCount] = tileData[arrayPos].screenPosition + new Vector2(tilePixelSize, tilePixelSize);
						botPointCount++;
					}
					if(r == maxDims.y+1 && topPointCount <= maxDims.x)
					{
						topPoints[topPointCount] = tileData[arrayPos].screenPosition + new Vector2(tilePixelSize, 0);
						topPointCount++;
					}
					if(c == 0 && leftPointCount <= maxDims.y)
					{
						leftPoints[leftPointCount] = tileData[arrayPos].screenPosition + new Vector2(tilePixelSize, tilePixelSize);
						leftPointCount++;
					}
					if(c == maxDims.x+1 && rightPointCount <= maxDims.y)
					{
						rightPoints[rightPointCount] = tileData[arrayPos].screenPosition + new Vector2(0, tilePixelSize);
						rightPointCount++;
					}
				}
				else
				{
					tileData[arrayPos].type = Tile.Type.Main;
					tileData[arrayPos].SetPoints(Tile.Shape.Empty, false, false);
				}

				arrayPos++;
			}
		}



		return tileData;
	}

	void UpdateTileBrush()
	{
		if (Input.GetKeyDown("1"))
		{
			activeShape = Tile.Shape.HalfTri;
			activeBrushIcon = brushIconHalfTri;
		}
		if (Input.GetKeyDown("2"))
		{
			activeShape = Tile.Shape.SharpTriHor;
			activeBrushIcon = brushIconSharpTriHor;
		}
		if (Input.GetKeyDown("3"))
		{
			activeShape = Tile.Shape.SharpTriVert;
			activeBrushIcon = brushIconSharpTriVert;
		}
		if (Input.GetKeyDown("4"))
		{
			activeShape = Tile.Shape.CornerSquare;
			activeBrushIcon = brushIconCornerSquare;
		}
		if (Input.GetKeyDown("5"))
		{
			activeShape = Tile.Shape.FlatRect;
			activeBrushIcon = brushIconFlatRect;
		}
		if (Input.GetKeyDown("6"))
		{
			activeShape = Tile.Shape.FatTriHor;
			activeBrushIcon = brushIconFatTriHor;
		}
		if (Input.GetKeyDown("7"))
		{
			activeShape = Tile.Shape.FatTriVert;
			activeBrushIcon = brushIconFatTriVert;
		}
		if (Input.GetKeyDown("8"))
		{
			activeShape = Tile.Shape.CornerSquareInvert;
			activeBrushIcon = brushIconCornerSquareInvert;
		}
		if (Input.GetKeyDown("9"))
		{
			activeShape = Tile.Shape.CornerTri;
			activeBrushIcon = brushIconCornerTri;
		}
		if (Input.GetKeyDown("0"))
		{
			activeShape = Tile.Shape.CornerTriInvert;
			activeBrushIcon = brushIconCornerTriInvert;
		}

		Sprite activeBrushSprite = activeBrushIcon.GetComponent<Image>().sprite;

		brushSelector.position = activeBrushIcon.transform.position;
		brushActiveQ.GetComponent<Image>().sprite = activeBrushSprite;
		brushActiveW.GetComponent<Image>().sprite = activeBrushSprite;
		brushActiveS.GetComponent<Image>().sprite = activeBrushSprite;
		brushActiveA.GetComponent<Image>().sprite = activeBrushSprite;

	}


	//.GetComponent<Image>().sprite

	void PaletteSwap(int n = 0)
	{
		Palette palette = palette1;
		if(n == 0)
		{
			n = Random.Range(1, 5);
		}
		if (n == 1)
		{
			palette = palette1;
		}
		if (n == 2)
		{
			palette = palette2;
		}
		if (n == 3)
		{
			palette = palette3;
		}
		if (n == 4)
		{
			palette = palette4;
		}

		gridColor = palette.levelTile;
		gridColor.a = .2f;
		shapeColor = palette.levelTile;

		foreach(Image s in tileSprites)
		{
			s.color = palette.levelTile;
		}
		foreach(Image b in bgSprites)
		{
			b.color = palette.levelSky;
		}
		foreach(Image x in boxSprites)
		{
			x.color = palette.levelBG[1];
		}
		foreach(Image f in frameSprites)
		{
			f.color = palette.levelBG[0];
		}
	}

*/
}



[System.Serializable]
public class Frame
{
	public Vector2 dims;
	public Edge edge;
	
	public enum Edge
	{
		Solid,
		Bottomless,
		Hazardous
	}

}


		/*
		tileData[0].type = Tile.Type.Corner;
		tileData[(int)maxDims.x+1].type = Tile.Type.Corner;
		tileData[tileData.Length-1].type = Tile.Type.Corner;
		tileData[tileData.Length-1-((int)maxDims.x+1)].type = Tile.Type.Corner;
		*/