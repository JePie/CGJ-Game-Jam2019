using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundSlider;
    [SerializeField] TextMeshProUGUI musicValue;
    [SerializeField] TextMeshProUGUI soundValue;

    [Space]

    [SerializeField] TMP_Dropdown textSpeedDropdown;

    void Start()
    {
        InitSliderValues();
        InitTextSpeed();
    }

    void InitSliderValues()
    {
        //init. slider values to PlayerPrefs (default: max slider value)
        musicSlider.value = PlayerPrefs.GetInt("musicVolume", (int)musicSlider.maxValue);
        soundSlider.value = PlayerPrefs.GetInt("soundVolume", (int)soundSlider.maxValue);
    }

    void InitTextSpeed()
    {
        //init. text speed value to PlayerPrefs
        //0: Fast; 1: Medium; 2: Slow
        textSpeedDropdown.value = PlayerPrefs.GetInt("textSpeed", 1);
    }

    //called when user changes musicSlider value
    public void OnMusicSliderChange()
    {
        PlayerPrefs.SetInt("musicVolume", (int)musicSlider.value);
        musicValue.text = musicSlider.value.ToString();
    }

    //called when user changes soundSlider value
    public void OnSoundSliderChange()
    {
        PlayerPrefs.SetInt("soundVolume", (int)soundSlider.value);
        soundValue.text = soundSlider.value.ToString();
    }

    //called when user changes textSpeedDropdown value
    public void OnTextSpeedValueChange()
    {
        PlayerPrefs.SetInt("textSpeed", textSpeedDropdown.value);
    }

    //called when user presses the back button
    public void BackToMainMenu(Canvas mainMenuCanvas)
    {
        mainMenuCanvas.enabled = true;
        GetComponent<Canvas>().enabled = false;
    }
}