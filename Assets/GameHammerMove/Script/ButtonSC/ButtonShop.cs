using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonShop : MonoBehaviour
{
    [SerializeField] private Canvas enCanvas;
    [SerializeField] private Button buttonShop;
    private void Start()
    {
        if (buttonShop != null)
        {
            buttonShop.onClick.AddListener(EnableCanvas);
        }
    }

    public void EnableCanvas()
    {
        enCanvas.gameObject.SetActive(true);
    }
}
