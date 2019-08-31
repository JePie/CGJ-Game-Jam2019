using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    GameController gameController;

    void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }

    //Resume Button Function buttons (public)
    public void SelectResume()
    {
        gameController.SetPausedState(false);
    }
    //Settings Menu Button Function (public)
    public void SelectSettings(Canvas settingsMenu)
    {
        settingsMenu.enabled = true;
        GetComponent<Canvas>().enabled = false;
    }
    //Return to Main Menu Function (public)
    public void SelectMainMenu()
    {
        //load Main Menu Scene
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
    }
}
