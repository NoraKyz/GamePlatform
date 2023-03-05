using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button playButton;

    void Awake()
    {
        playButton.onClick.AddListener(OnClickPlayButton);
    }

    void OnClickPlayButton()
    {
        SceneManager.LoadScene("GamePlay");
    }
}
