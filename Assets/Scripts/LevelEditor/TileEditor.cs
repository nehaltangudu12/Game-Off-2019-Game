using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TileEditor
{

	
	public Shape[] tileShapes;

	public MeshFilter tileMesh;

	Shape activeShape;
	Level level;

	public Transform highlightTransform;
	public Renderer highlightRenderer;

	public Color shapeColor;
	public Color activeShapeColor;

	public bool hideGrid;

	public GameObject keyQ;
	public Vector3 useFlipQ;
	public Vector3 useTurnsQ;
	public GameObject keyW;
	public Vector3 useFlipW;
	public Vector3 useTurnsW;
	public GameObject keyS;
	public Vector3 useFlipS;
	public Vector3 useTurnsS;
	public GameObject keyA;
	public Vector3 useFlipA;
	public Vector3 useTurnsA;

	public GameObject gridLine;
	LineRenderer[] gridLines;


	public void InitializeEditor(Level level)
	{
		this.level = level;
		activeShape = tileShapes[0];
		scrolledTile = level.tiles[0];
		highlightTransform.localScale = new Vector3(level.tilePixelSize, level.tilePixelSize, 1);
		UpdateShapeUI();
		GenerateGrid();
	}

	public void GenerateGrid()
	{
		gridLines = new LineRenderer[(int)level.gridDims.x + (int)level.gridDims.y - 2];
		
		float leftX = level.tilePixelSize + level.tilePixelOffset.x;
		float rightX = (level.gridDims.x+1) * level.tilePixelSize + level.tilePixelOffset.x;
		float botY = level.tilePixelSize + level.tilePixelOffset.y;
		float topY = (level.gridDims.y+1) * level.tilePixelSize + level.tilePixelOffset.y;
		
		int currentRenderer = 0;

		for (int c = 2; c < level.gridDims.x + 1; c++)
		{
			float cPos = c * level.tilePixelSize + level.tilePixelOffset.x;
			Vector2 pointA = new Vector2(cPos, topY);
			Vector2 pointB = new Vector2(cPos, botY);
			gridLines[currentRenderer] = GameObject.Instantiate(gridLine).GetComponent<LineRenderer>();
			gridLines[currentRenderer].SetPositions(new Vector3[]{pointA, pointB});
			currentRenderer++;
		}
		for (int r = 2; r < level.gridDims.y + 1; r++)
		{
			float rPos = r * level.tilePixelSize + level.tilePixelOffset.y;
			Vector2 pointA = new Vector2(leftX, rPos);
			Vector2 pointB = new Vector2(rightX, rPos);
			gridLines[currentRenderer] = GameObject.Instantiate(gridLine).GetComponent<LineRenderer>();
			gridLines[currentRenderer].SetPositions(new Vector3[]{pointA, pointB});
			currentRenderer++;
		}
	}


	public void UpdateActiveShape()
	{
		bool shapeChanged = false;
		foreach(Shape s in tileShapes)
		{
			if(Input.GetKey(s.inputKey))
			{
				if (s!= activeShape)
				{
					shapeChanged = true;
					activeShape = s;
					
				}
			}
		}

		if(shapeChanged)
		{
			UpdateShapeUI();
		}
	}

	public void UpdateShapeUI()
	{
		foreach(Shape h in tileShapes)
		{
			h.indicator.color = shapeColor;
		}
		activeShape.indicator.color = activeShapeColor;
		keyQ.GetComponent<Image>().sprite = activeShape.indicator.sprite;
		keyW.GetComponent<Image>().sprite = activeShape.indicator.sprite;
		keyS.GetComponent<Image>().sprite = activeShape.indicator.sprite;
		keyA.GetComponent<Image>().sprite = activeShape.indicator.sprite;
		if(activeShape.useTurns)
		{
			keyQ.transform.eulerAngles = useTurnsQ;
			keyW.transform.eulerAngles = useTurnsW;
			keyS.transform.eulerAngles = useTurnsS;
			keyA.transform.eulerAngles = useTurnsA;
		}
		else
		{
			keyQ.transform.eulerAngles = useFlipQ;
			keyW.transform.eulerAngles = useFlipW;
			keyS.transform.eulerAngles = useFlipS;
			keyA.transform.eulerAngles = useFlipA;
		}
	}


	public void ToggleGridLines()
	{
		if (Input.GetKeyDown("g"))
		{
			hideGrid = !hideGrid;
			foreach(Renderer g in gridLines)
			{
				g.enabled = !hideGrid;
			}
		}
	}


	Tile scrolledTile;
	bool scrolling;

	public void UpdateScrolledTile()
	{
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		bool exitScroll = false;
		if (mousePos.x >= level.tilePixelOffset.x + level.tilePixelSize && mousePos.x <= level.tilePixelOffset.x*-1 - level.tilePixelSize && mousePos.y >= level.tilePixelOffset.y + level.tilePixelSize && mousePos.y <= level.tilePixelOffset.y*-1 - level.tilePixelSize)
		{
			Tile currentTile = level.PointToTile(mousePos);
			if (!currentTile.isBorder)
			{
				if(!scrolling)
				{
					highlightRenderer.enabled = true;
				}
				scrolling = true;

				if (currentTile != scrolledTile)
				{
					scrolledTile = currentTile;
					highlightTransform.position = new Vector3(scrolledTile.screenPos.x, scrolledTile.screenPos.y, highlightTransform.position.z);
				}
		
				bool shapeBrush = false;
				
				Tile.Key key = Tile.Key.Q;

				if(Input.GetKey("q"))
				{
					shapeBrush = true;
				}
				if(Input.GetKey("w"))
				{
					shapeBrush = true;
					key = Tile.Key.W;
				}
				if(Input.GetKey("s"))
				{
					shapeBrush = true;
					key = Tile.Key.S;
				}
				if(Input.GetKey("a"))
				{
					shapeBrush = true;
					key = Tile.Key.A;
				}

				bool updateMesh = false;

				if (shapeBrush)
				{
					scrolledTile.ClearTile();
					scrolledTile.ShapeTile(activeShape, key);
					updateMesh = true;
				}

				if (Input.GetKey("e"))
				{
					scrolledTile.FillTile();
					updateMesh = true;
				}
				if (Input.GetKey("d"))
				{
					scrolledTile.ClearTile();
					updateMesh = true;
				}

				if (updateMesh)
				{
					tileMesh.mesh = level.TilesToMesh();
				}
			}
			else
			{
				scrolling = false;
			}
		}
		else
		{
			scrolling = false;
		}
		
		if (exitScroll)
		{
			highlightRenderer.enabled = false;
		}
	}




	/*
	public GameObject brushIconHalfTri;
	public GameObject brushIconSharpTriHor;
	public GameObject brushIconSharpTriVert;
	public GameObject brushIconFatTriHor;
	public GameObject brushIconFatTriVert;
	public GameObject brushIconFlatRect;
	public GameObject brushIconCornerSquare;
	public GameObject brushIconCornerSquareInvert;
	public GameObject brushIconCornerTri;
	public GameObject brushIconCornerTriInvert;

	public GameObject brushActiveQ;
	public GameObject brushActiveA;
	public GameObject brushActiveS;
	public GameObject brushActiveW;

	GameObject activeBrushIcon;

	public RectTransform brushSelector;
	*/
}
