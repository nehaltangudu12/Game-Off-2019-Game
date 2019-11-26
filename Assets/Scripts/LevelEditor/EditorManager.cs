using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorManager : MonoBehaviour
{

	public enum Editing{Tiles,Objects,Level}
	Editing editMode;

	public TileEditor tileEditor;

	public VisualsEditor visualsEditor;


	Level currentLevel;

	public Vector2 camViewSize;
	public Vector2 defaultLevelDims;

	void NewLevel()
	{
		currentLevel = new Level(defaultLevelDims, camViewSize);
		tileEditor.tileMesh.mesh = currentLevel.TilesToMesh();
	}

	void Start ()
	{
		NewLevel();
		visualsEditor.PaletteSwap();
		StartTileMode();
	}


	void StartTileMode ()
	{
		editMode = Editing.Tiles;
		tileEditor.InitializeEditor(currentLevel);
	}



	void Update ()
	{
		
		//check for mode switches


		//input update loops for each mode:

		if(editMode == Editing.Tiles)
		{
			tileEditor.UpdateActiveShape();
			tileEditor.UpdateScrolledTile();
			tileEditor.ToggleGridLines();
		}



	}


	



}
