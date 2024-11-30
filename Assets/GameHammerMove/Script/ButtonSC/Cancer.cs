using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cancer : MonoBehaviour
{
    [SerializeField] private Canvas disShop;
    [SerializeField] private Button buttonCancer;
    private void Start()
    {
        if (buttonCancer != null)
        {
            buttonCancer.onClick.AddListener(DisableShop);
        }
    }

    public void DisableShop()
    {
        disShop.gameObject.SetActive(false);
    }
}
