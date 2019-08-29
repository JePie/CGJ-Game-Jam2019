using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] Canvas[] pauseMenus;
    public bool paused { get; private set; }

    [SerializeField] 

    void Start()
    {

    }
    
    void Update()
    {
        HandlePauseInput();
    }

    void HandlePauseInput()
    {
        if (Input.GetButtonDown("Pause"))
        {
            SetPausedState(!paused);
        }
    }

    public void SetPausedState(bool state)
    {
        paused = state;
        UpdatePausedState();
    }

    //handle pause functions
    void UpdatePausedState()
    {
        if (paused)
        {
            pauseMenus[0].enabled = true;
            Time.timeScale = 0;
        }
        else
        {
            foreach (Canvas c in pauseMenus)
            {
                c.enabled = false;
            }
        }
    }

    public void RestartLevel()
    {
        //
    }

    //immediately pause game if application is unfocused (excluding inside editor)
    void OnApplicationFocus(bool focus)
    {
#if !UNITY_EDITOR
        if (!focus)
        {
            SetPauseState(true);
        }
#endif
    }
}