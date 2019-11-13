using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile
{
	public enum Type
	{
		Border,
		Empty,
		IsShape
	}

	public Type type;

	public bool inFrame;
	public Vector2 coord;
	public Vector2 screenPosition;
	public int arrayPos;
	public Vector2[] points;

	public enum Shape
	{
		Empty,
		Full,
		HalfTri,
		SharpTriLeft,
		SharpTriRight,
		FatTriLeft,
		FatTriRight,
		FlatRect,
		CornerFull,
		CornerEmpty
	}

	public void CheckFrameState(Vector2 frameSize, Vector2 frameCoord)
	{

	}


	public void SetPoints (Shape shape, int turns = 0)
	{
		points = new Vector2[0];

		Vector2 topLeft = new Vector2(0, 1);
		Vector2 topRight = new Vector2(1, 1);
		Vector2 botLeft = new Vector2(0, 0);
		Vector2 botRight = new Vector2(1, 0);
		Vector2 midTop = new Vector2(0.5f, 1);
		Vector2 midBot = new Vector2(0.5f, 0);
		Vector2 midRight = new Vector2(1, 0.5f);
		Vector2 midLeft = new Vector2(0, 0.5f);
		Vector2 midMid = new Vector2(0.5f, 0.5f);

		//points connect in order, final point connects to first point to complete the shape
		if (shape == Shape.Full)
		{
			points = new Vector2[]{botLeft, botRight, topRight, topLeft};
		}
		if (shape == Shape.HalfTri)
		{
			points = new Vector2[]{botLeft, topRight, botRight};
		}
		if (shape == Shape.SharpTriLeft)
		{
			points = new Vector2[]{midRight, botRight, botLeft};
		}
		if (shape == Shape.SharpTriRight)
		{
			points = new Vector2[]{midLeft, botLeft, botRight};
		}
		if (shape == Shape.FatTriLeft)
		{
			points = new Vector2[]{midLeft, botLeft, botRight, topRight};
		}
		if (shape == Shape.FatTriRight)
		{
			points = new Vector2[]{midRight, botRight, botLeft, topLeft};
		}
		if (shape == Shape.FlatRect)
		{
			points = new Vector2[]{midLeft, midRight, botRight, botLeft};
		}
		if (shape == Shape.CornerFull)
		{
			points = new Vector2[]{midLeft, midMid, midBot, botLeft};
		}
		if (shape == Shape.CornerEmpty)
		{
			points = new Vector2[]{topLeft, midTop, midMid, midRight, botRight, botLeft};
		}

		if (turns != 0)
		{
			float rad = Mathf.Deg2Rad * (90 * turns);
			float c = Mathf.Cos(rad);
			float s = Mathf.Sin(rad);
			for (int i = 0; i < points.Length; i++)
			{
				float newX = (points[i].x -.5f) * c - (points[i].y-.5f) * s;
				float newY = (points[i].y -.5f) * c + (points[i].x-.5f) * s;
				points[i] = new Vector2(newX + .5f, newY + .5f);
			}
		}


	}
}
