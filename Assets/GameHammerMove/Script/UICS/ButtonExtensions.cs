using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ButtonExtensions 
{
    public static void AddEventListener<T> (this Button button, T param, Action<T> Onclick)
    {
        button.onClick.AddListener(delegate ()
        {
            Onclick(param);
        });
    }
}
