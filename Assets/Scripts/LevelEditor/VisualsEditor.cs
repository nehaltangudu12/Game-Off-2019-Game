using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class VisualsEditor
{
	public Palette world1;
	public Palette world2;
	public Palette world3;
	public Palette world4;
	public Renderer tileMesh;

	public Image fullBG;
	public Image skyBG;


	public void PaletteSwap(int n = 0)
	{
		Palette palette = world1;
		if(n == 0)
		{
			n = Random.Range(1, 5);
		}
		if (n == 1)
		{
			palette = world1;
		}
		if (n == 2)
		{
			palette = world2;
		}
		if (n == 3)
		{
			palette = world3;
		}
		if (n == 4)
		{
			palette = world4;
		}

		tileMesh.material.color = palette.levelBG[0];
		skyBG.color = palette.levelBG[1];
		fullBG.color = palette.levelSky;
	}



}
