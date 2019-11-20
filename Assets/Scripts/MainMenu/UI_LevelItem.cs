using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LevelItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer VisualLevelItem = null;
    [SerializeField] private SpriteRenderer VisualLevelContent = null;

    public Vector2 Size => VisualLevelItem.bounds.size;

    public bool IsHovered { get; private set; }

    private Color _initColor = Color.black;

    public void Init ()
    {
        _initColor = VisualLevelContent.color;
    }

    public void UpdateItem (bool isHovered)
    {
        VisualLevelContent.color = isHovered ? Color.white : _initColor;
    }
}