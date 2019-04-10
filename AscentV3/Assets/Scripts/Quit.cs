using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Quit : MonoBehaviour {

    public void QuitApp()
    {
        Application.Quit();
    }

    public void OnClickFfs()
    {
        SceneManager.LoadScene("Ascent");
    }


    public void OnClickMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
