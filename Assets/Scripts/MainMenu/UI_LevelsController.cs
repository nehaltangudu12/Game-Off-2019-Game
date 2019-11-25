using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using GhAyoub.InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_LevelsController : MonoBehaviour
{
    [SerializeField] private MainMenuCameraController MainMenuCameraController;
    [SerializeField] private UI_LevelItem[] LevelItems;

    private PlayerInput _inputInstance = null;
    private SceneController _sceneControl = null;
    private UI_LevelItem _currentHoveredItem = null;

    private void Start ()
    {
        _inputInstance = PlayerInput.Instance;
        _sceneControl = SceneController.Instance;

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
            var diff = new Vector3 (mousePos.x, mousePos.y, 0f) - new Vector3 (itemPos.x - item.Size.x / 2, itemPos.y - item.Size.y / 2, 0f);

            var ishovered = Vector3.SqrMagnitude (diff) < (2f * 2f);

            item.UpdateItem (ishovered);

            if (ishovered) _currentHoveredItem = item;
        }

        if (_inputInstance.Data.MouseClick)
        {
            if (_currentHoveredItem != null)
            {
                ChargeAScene ();
            }
        }

    }

    void ChargeAScene ()
    {
        MainMenuCameraController.DepthOfFieldAnim ();
        Camera.main.DOOrthoSize (1.5f, 3f).OnComplete (() =>
        {
           _sceneControl.LoadSceneAsync(1);
        });
    }
}