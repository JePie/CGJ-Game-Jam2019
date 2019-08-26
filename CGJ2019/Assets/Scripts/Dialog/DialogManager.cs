using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    Canvas dialogCanvas;

    [SerializeField] TextMeshProUGUI textDisplay;
    [SerializeField] string[] dialog;
    int dialogIndex;
    int textSpeed;

    IEnumerator textPrintCoroutine;

    void Awake()
    {
        dialogCanvas = GetComponent<Canvas>();

        //init. <textSpeed> to PlayerPrefs
        //PlayerPrefs updated in SettingsMenu.cs
        textSpeed = PlayerPrefs.GetInt("textSpeed");
    }

    void Start()
    {
        textPrintCoroutine = PrintText();
        StartCoroutine(textPrintCoroutine);
    }

    //appends each character in <dialog[dialogIndex]> one by one, at a speed based on <textSpeed>
    IEnumerator PrintText()
    {
        foreach (char character in dialog[dialogIndex].ToCharArray())
        {
            textDisplay.text += character;
            yield return new WaitForSeconds((textSpeed + 1) * 0.02f);
        }
    }

    void Update()
    {
        HandleNextDialogInput();
    }

    void HandleNextDialogInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //if pressed before all text has finished displaying
            if (textDisplay.text.Length < dialog[dialogIndex].Length)
            {
                //stop printing the text through the coroutine
                StopCoroutine(textPrintCoroutine);

                //display all text right away
                textDisplay.text = dialog[dialogIndex];
            }
            else
            {
                //if there is more dialog to print
                if (dialogIndex < dialog.Length - 1)
                {
                    //reset text, incrememt <dialogIndex> start printing again
                    dialogIndex++;
                    textDisplay.text = "";
                    textPrintCoroutine = PrintText();
                    StartCoroutine(textPrintCoroutine);
                }
                else
                {
                    //reset text once dialog has completed
                    textDisplay.text = "";
                    dialogCanvas.enabled = false;
                }
            }
        }
    }

}