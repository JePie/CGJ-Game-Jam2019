using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void SelectPlay()
    {
        //load first level
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Level 1");
    }

    public void SelectSettings(Canvas settingsMenu)
    {
        settingsMenu.enabled = true;
        GetComponent<Canvas>().enabled = false;
    }

    public void SelectQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else 
        Application.Quit();
#endif
    }
}