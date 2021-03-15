﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Toggle toggleMode;
    private void Start()
    {
        Time.timeScale = 1;
        string mode = PlayerPrefs.GetString("@mode", "");
        if (mode == "")
        {
            mode = "Normal Mode";
            PlayerPrefs.SetString("@mode", mode);
        }
        Text modeName = toggleMode.GetComponentInChildren<Text>();
        modeName.text = mode;
        toggleMode.isOn = mode == "Hard Mode";
    }
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Exit()
    {
        Application.Quit();
    }

    public void ChangeMode()
    {
        bool hardMode = toggleMode.isOn;
        Text modeName = toggleMode.GetComponentInChildren<Text>();
        modeName.text = hardMode ? "Hard Mode" : "Normal Mode";
        PlayerPrefs.SetString("@mode", modeName.text);
    }
}
