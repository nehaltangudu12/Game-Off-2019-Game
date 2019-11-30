using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HowToPlay : MonoBehaviour
{
    [SerializeField] private Button CloseBtn = null;

    private MainMenuController _menuControl;

    internal void Init (MainMenuController menuControl)
    {
        _menuControl = menuControl;

        CloseBtn.onClick.AddListener (() =>
        {
            _menuControl.CloseHTP();
        });

    }

    internal void Close ()
    {
        this.gameObject.SetActive (false);
    }
}