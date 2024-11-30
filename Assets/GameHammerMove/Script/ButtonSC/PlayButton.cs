using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private Canvas disCanvas;
    [SerializeField] private Button button;
    private void Start()
    {
        if (button != null)
        {
            button.onClick.AddListener(DisableCanvas);
        }
    }

    public void DisableCanvas()
    {
        disCanvas.gameObject.SetActive(false);
    }
}
