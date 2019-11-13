using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
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

	public float tilePixelSize;
	public bool showGrid = true;
	public RectTransform mapBG;
	public RectTransform borderBG;
	bool loading = false;
	Tile[] currentMap;
	Tile scrolledTile;

	public GameObject brushIconHalfTri;
	public GameObject brushIconSharpTriLeft;
	public GameObject brushIconSharpTriRight;
	public GameObject brushIconFatTriLeft;
	public GameObject brushIconFatTriRight;
	public GameObject brushIconFlatRect;
	public GameObject brushIconCornerFull;
	public GameObject brushIconCornerEmpty;

	public GameObject brushActiveQ;
	public GameObject brushActiveA;
	public GameObject brushActiveS;
	public GameObject brushActiveW;

	GameObject activeBrushIcon;

	public RectTransform brushSelector;

	Vector2 boundsTopRight;
	Vector2 boundsTopLeft;
	Vector2 boundsBotRight;
	Vector2 boundsBotLeft;



	Tile.Shape activeShape = Tile.Shape.HalfTri;


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
		}
		else
		{
			//load map 
		}
	}

	



	void Update()
	{
		UpdateTileBrush();
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		if (mousePos.x >= offset.x + tilePixelSize && mousePos.x <= offset.x*-1 - tilePixelSize && mousePos.y >= offset.y + tilePixelSize && mousePos.y <= offset.y*-1 - tilePixelSize)
		{
			Tile currentTile = WorldToGrid(mousePos, currentMap);
			if (currentTile != scrolledTile && currentTile.type != Tile.Type.Border)
			{
				scrolledTile = currentTile;
			}

			Debug.DrawLine(scrolledTile.screenPosition, new Vector2(scrolledTile.screenPosition.x + tilePixelSize, scrolledTile.screenPosition.y), scrollColor);
			Debug.DrawLine(scrolledTile.screenPosition, new Vector2(scrolledTile.screenPosition.x, scrolledTile.screenPosition.y + tilePixelSize), scrollColor);
			Debug.DrawLine(scrolledTile.screenPosition + new Vector2(tilePixelSize, tilePixelSize), new Vector2(scrolledTile.screenPosition.x + tilePixelSize, scrolledTile.screenPosition.y), scrollColor);
			Debug.DrawLine(scrolledTile.screenPosition + new Vector2(tilePixelSize, tilePixelSize), new Vector2(scrolledTile.screenPosition.x, scrolledTile.screenPosition.y + tilePixelSize), scrollColor);
		
			if (scrolledTile.type != Tile.Type.Border)
			{
				bool shapeBrush = false;
				int turns = 0;
				if(Input.GetKey("q"))
				{
					shapeBrush = true;
					turns = 0;
				}
				if(Input.GetKey("w"))
				{
					shapeBrush = true;
					turns = 3;
				}
				if(Input.GetKey("s"))
				{
					shapeBrush = true;
					turns = 2;
				}
				if(Input.GetKey("a"))
				{
					shapeBrush = true;
					turns = 1;
				}
				if (shapeBrush)
				{
					scrolledTile.type = Tile.Type.IsShape;
					scrolledTile.SetPoints(activeShape, turns);
				}

				if (Input.GetKey("e"))
				{
					scrolledTile.type = Tile.Type.IsShape;
					scrolledTile.SetPoints(Tile.Shape.Full);
				}
				if (Input.GetKey("d"))
				{
					scrolledTile.type = Tile.Type.Empty;
					scrolledTile.SetPoints(Tile.Shape.Empty);
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
			Debug.DrawLine(boundsTopLeft, boundsTopRight, shapeColor);
			Debug.DrawLine(boundsTopLeft, boundsBotLeft, shapeColor);
			Debug.DrawLine(boundsBotRight, boundsTopRight, shapeColor);
			Debug.DrawLine(boundsBotRight, boundsBotLeft, shapeColor);
			Debug.DrawLine(boundsTopLeft + new Vector2(-tilePixelSize,tilePixelSize), boundsTopRight + new Vector2(tilePixelSize,tilePixelSize), shapeColor);
			Debug.DrawLine(boundsTopLeft + new Vector2(-tilePixelSize,tilePixelSize), boundsBotLeft + new Vector2(-tilePixelSize,-tilePixelSize), shapeColor);
			Debug.DrawLine(boundsBotRight + new Vector2(tilePixelSize,-tilePixelSize), boundsTopRight + new Vector2(tilePixelSize,tilePixelSize), shapeColor);
			Debug.DrawLine(boundsBotRight + new Vector2(tilePixelSize,-tilePixelSize), boundsBotLeft + new Vector2(-tilePixelSize,-tilePixelSize), shapeColor);
		}
		
		if (debugTileShapes)
		{
			foreach (Tile t in currentMap)
			{
				if (t.type == Tile.Type.IsShape)
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
					tileData[arrayPos].SetPoints(Tile.Shape.Full);

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
					tileData[arrayPos].type = Tile.Type.Empty;
				}

				arrayPos++;
			}
		}

		/*
		tileData[0].type = Tile.Type.Corner;
		tileData[(int)maxDims.x+1].type = Tile.Type.Corner;
		tileData[tileData.Length-1].type = Tile.Type.Corner;
		tileData[tileData.Length-1-((int)maxDims.x+1)].type = Tile.Type.Corner;
		*/

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
			activeShape = Tile.Shape.SharpTriLeft;
			activeBrushIcon = brushIconSharpTriLeft;
		}
		if (Input.GetKeyDown("3"))
		{
			activeShape = Tile.Shape.SharpTriRight;
			activeBrushIcon = brushIconSharpTriRight;
		}
		if (Input.GetKeyDown("4"))
		{
			activeShape = Tile.Shape.CornerFull;
			activeBrushIcon = brushIconCornerFull;
		}
		if (Input.GetKeyDown("5"))
		{
			activeShape = Tile.Shape.FlatRect;
			activeBrushIcon = brushIconFlatRect;
		}
		if (Input.GetKeyDown("6"))
		{
			activeShape = Tile.Shape.FatTriLeft;
			activeBrushIcon = brushIconFatTriLeft;
		}
		if (Input.GetKeyDown("7"))
		{
			activeShape = Tile.Shape.FatTriRight;
			activeBrushIcon = brushIconFatTriRight;
		}
		if (Input.GetKeyDown("8"))
		{
			activeShape = Tile.Shape.CornerEmpty;
			activeBrushIcon = brushIconCornerEmpty;
		}

		Sprite activeBrushSprite = activeBrushIcon.GetComponent<Image>().sprite;

		brushSelector.position = activeBrushIcon.transform.position;
		brushActiveQ.GetComponent<Image>().sprite = activeBrushSprite;
		brushActiveW.GetComponent<Image>().sprite = activeBrushSprite;
		brushActiveS.GetComponent<Image>().sprite = activeBrushSprite;
		brushActiveA.GetComponent<Image>().sprite = activeBrushSprite;

	}

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