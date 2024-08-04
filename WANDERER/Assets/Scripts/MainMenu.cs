using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string nextSceneName = "SnowMoutain";
    public void Play()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}