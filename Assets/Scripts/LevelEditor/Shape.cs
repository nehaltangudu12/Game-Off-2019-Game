using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class Shape
{
	public string name;
	public int[] tris;
	public Vector2[] verts;
	public bool useTurns;
	public bool isFull;
	public bool isEmpty;
	public string inputKey;
	public Image indicator;
}