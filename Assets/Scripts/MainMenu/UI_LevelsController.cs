using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using GhAyoub.InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_LevelsController : MonoBehaviour
{
    [SerializeField] private MainMenuCameraController MainMenuCameraController;
    [SerializeField] private UI_LevelItem[] LevelItems;

    private MainMenuController _menuControl = null;
    private PlayerInput _inputInstance = null;
    private SceneController _sceneControl = null;
    private UI_LevelItem _currentHoveredItem = null;

    private void Start ()
    {
        _inputInstance = PlayerInput.Instance;
        _sceneControl = SceneController.Instance;
        _menuControl = MainMenuController.Instance;

        foreach (var item in LevelItems)
        {
            item.Init ();
        }
    }

    void Update ()
    {
        if (_menuControl.IsSettingsActive) return;

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
            Interact (_currentHoveredItem);
        }
    }

    void Interact (UI_LevelItem item)
    {
        if (_currentHoveredItem == null) return;

        var index = LevelItems.ToList ().IndexOf (item);

        switch (index)
        {
            case 0:
                _menuControl.OpenHTP ();
                break;

            case 1:
                ChargeAScene ();
                break;

            case 2:
                _menuControl.OpenSettings ();
                break;

            default:
                break;
        }
    }

    void ChargeAScene ()
    {
        MainMenuCameraController.DepthOfFieldAnim ();
        _sceneControl.LoadSceneAsync (1);
        // Camera.main.DOOrthoSize (1.5f, 3f).OnComplete (() =>
        // {

        // });
    }
}