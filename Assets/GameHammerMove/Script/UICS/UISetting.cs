using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UISetting : MonoBehaviour
{

    public GameObject Setting;

    


    public void Switch()
    {
        if (Setting.activeSelf == false) Setting.SetActive(true);
        else Setting.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene("new");
    }
}
