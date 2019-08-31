using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void SelectPlay(int firstLevelIndex)
    {
        Time.timeScale = 1;
        //load first level
        SceneManager.LoadSceneAsync(firstLevelIndex);
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