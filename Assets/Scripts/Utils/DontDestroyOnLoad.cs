using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : SingletonMB<DontDestroyOnLoad>
{
    new void Awake ()
    {
        DontDestroyOnLoad (this);
    }
}