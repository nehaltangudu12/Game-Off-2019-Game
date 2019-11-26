using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tile
{

	
	public bool isBorder;
	public int index;
	public Vector2 coord;
	public bool isEmpty = true;

	public Tile (bool isBorder, int index, Vector2 coord)
	{
		this.isBorder = isBorder;
		this.index = index;
		this.coord = coord;
		if (isBorder)
		{
			FillTile();
		}
		else
		{
			ClearTile();
		}
	}

	public enum Key
	{
		Q,W,S,A
	}
	


	public Vector2 screenPos;

	public void SetScreenPos (float tilePixelSize, Vector2 offset) // find the bottom left anchor of the tile in world points
	{
		screenPos = new Vector2(coord.x * tilePixelSize + offset.x, coord.y * tilePixelSize + offset.y);
	}

	public Vector2[] verts;
	public int[] tris;

	
	public void ClearTile ()
	{
		isEmpty = true;
		verts = new Vector2[0];
		tris = new int[0];
	}

	public void FillTile ()
	{
		isEmpty = false;
		verts = new Vector2[4]{
			new Vector2(0,0),
			new Vector2(0,1),
			new Vector2(1,1),
			new Vector2(1,0)};
		tris = new int[6]{
			0,
			1,
			2,
			2,
			3,
			0};
	}




	public void ShapeTile (Shape shape, Key key) // set the tile's verts using a predefined shape
	{
		bool flipHor = false;
		bool flipVer = false;
		int turns = 0;
		ClearTile();
		isEmpty = false;
		if (key == Key.Q)
		{
			flipHor = false;
			flipVer = false;
			turns = 0;
		}
		if (key == Key.W)
		{
			flipHor = true;
			flipVer = false;
			turns = 3;
		}
		if (key == Key.S)
		{
			flipHor = true;
			flipVer = true;
			turns = 2;
		}
		if (key == Key.A)
		{
			flipHor = false;
			flipVer = true;
			turns = 1;
		}

		verts = new Vector2[shape.verts.Length];
		
		for(int s = 0; s < verts.Length; s++)
		{
			verts[s] = new Vector2(shape.verts[s].x, shape.verts[s].y);
		}

		if (flipHor && !shape.useTurns)
		{
			for(int h = 0; h < verts.Length; h++)
			{
				Vector2 newH = verts[h] * new Vector2(-1, 1) + new Vector2(1, 0);
				verts[h] = newH;
			}
		}

		if (flipVer && !shape.useTurns)
		{
			for(int v = 0; v < verts.Length; v++)
			{
				Vector2 newV = verts[v] * new Vector2(1, -1) + new Vector2(0, 1);
				verts[v] = newV;
			}
		}

		if (turns != 0 && shape.useTurns)
		{
			float rad = Mathf.Deg2Rad * (90 * turns);
			float c = Mathf.Cos(rad);
			float s = Mathf.Sin(rad);
			for (int i = 0; i < verts.Length; i++)
			{
				float newX = (verts[i].x -.5f) * c - (verts[i].y-.5f) * s;
				float newY = (verts[i].y -.5f) * c + (verts[i].x-.5f) * s;
				verts[i] = new Vector2(newX + .5f, newY + .5f);
			}
		}

		tris = shape.tris;
	}


	//gameplay vars
	bool inFrame;


}
