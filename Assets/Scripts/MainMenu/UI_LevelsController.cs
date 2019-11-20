using System.Collections;
using System.Collections.Generic;
using GhAyoub.InputSystem;
using UnityEngine;

public class UI_LevelsController : MonoBehaviour
{
    [SerializeField] private UI_LevelItem[] LevelItems;

    private PlayerInput _inputInstance;

    private void Start ()
    {
        _inputInstance = PlayerInput.Instance;

        foreach (var item in LevelItems)
        {
            item.Init ();
        }
    }

    void Update ()
    {
        var mousePos = _inputInstance.Data.MousePosition;
        for (int i = 0; i < LevelItems.Length; i++)
        {
            var item = LevelItems[i];
            var itemPos = LevelItems[i].transform.position;
            var ishovered = mousePos.x < itemPos.x && mousePos.x > itemPos.x - item.Size.x;

            item.UpdateItem (ishovered);
        }
    }
}